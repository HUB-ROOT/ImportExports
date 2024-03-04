// DataService.cs
using ImportExport.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace ImportExport.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;

        public DataService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Dictionary<string, List<string>> GetEntitySchema(string entity)
        {
            
            var entityTypeName = "ImportExport.Models." + entity;
            var entitySchema = new Dictionary<string, List<string>>();

            // Ensure the entityTypeName contains the full type name including namespace
            var entityType = _dbContext.Model.GetEntityTypes()
                .FirstOrDefault(e => e.ClrType.FullName == entityTypeName );

            if (entityType == null)
            {
                throw new ArgumentException($"Entity type '{entityTypeName}' not found in the DbContext.");
            }

            // Iterate over the properties of the entity and get the schema information
            foreach (var property in entityType.GetProperties())
            {
                var propertyName = property.Name;
                var propertyType = property.ClrType.Name;

                if (!entitySchema.ContainsKey(propertyName))
                {
                    entitySchema.Add(propertyName, new List<string>());
                }

                entitySchema[propertyName].Add(propertyType);
            }

            return entitySchema;
        }

        public List<string> FindRelatedEntities(string mainEntityName)
        {
            var relatedEntities = new List<string>();

            var entityType = _dbContext.Model.GetEntityTypes()
                .FirstOrDefault(e => e.ClrType.FullName == "ImportExport.Models." + mainEntityName);

            if (entityType != null)
            {
                // Iterate over navigation properties
                foreach (var navigation in entityType.GetNavigations())
                {
                    // Get the related entity type name
                    var relatedEntityTypeName = navigation.TargetEntityType.ClrType.Name;

                    // Check if the related entity implements IImportable interface
                    var relatedEntityType = navigation.TargetEntityType.ClrType;
                    if (typeof(IImportable).IsAssignableFrom(relatedEntityType))
                    {
                        // Add the related entity to the list
                        relatedEntities.Add(relatedEntityTypeName);
                    }
                }
            }

            return relatedEntities;
        }


    }
}

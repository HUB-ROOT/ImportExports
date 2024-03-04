using ImportExport.Models;
using ImportExport.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ImportExport.Controllers
{ 
    public class ExcelController : Controller
    {
        private readonly ExcelExportService _excelExportService; 
        private readonly DataService _dataservices;

        public ExcelController(ExcelExportService excelExportService, ApplicationDbContext dbContext, DataService dataService)
        {
            _excelExportService = excelExportService;
       
            _dataservices= dataService;
        }
         
        public IActionResult Export()
        {
            return View();
        }

        public IActionResult ExportEntities(string mainEntityName)
        {
            var zipMemoryStream = new MemoryStream();

            using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                var processedEntities = new HashSet<string>(); // Track processed entities globally
                ExportEntityAndRelated(mainEntityName, archive, processedEntities);
            }

            zipMemoryStream.Seek(0, SeekOrigin.Begin);

            return File(zipMemoryStream, "application/zip", $"{mainEntityName}_Export.zip");
        }

        private void ExportEntityAndRelated(string entityName, ZipArchive archive, HashSet<string> processedEntities)
        {
            if (processedEntities.Contains(entityName))
            {
                // Skip entities that have already been processed to avoid infinite recursion
                return;
            }

            var entityData = GetEntityData(entityName);
            var entitySchema = _dataservices.GetEntitySchema(entityName);

            // Generate Excel for the main entity
            var entityExcelBytes = _excelExportService.GenerateExcel(entityData, entitySchema, entityName);
            AddToZipArchive(archive, entityExcelBytes, $"{entityName}.xlsx");

            // Add the current entity to the processed entities set
            processedEntities.Add(entityName);

            // Find related sub-entities
            var relatedEntities = _dataservices.FindRelatedEntities(entityName);

            // Recursively export related entities
            foreach (var relatedEntity in relatedEntities)
            {
                ExportEntityAndRelated(relatedEntity, archive, processedEntities);
            }
        }


        private IEnumerable<object> GetEntityData(string entityName)
        {
            // Replace with your logic to get data based on the entity name
            // This is a placeholder and you should implement your own data retrieval logic.
            // You can use reflection to dynamically determine the entity type based on the name.
            // For simplicity, we are returning a list of objects here.
            return new List<object> { /* ... */ };
        }

        private void AddToZipArchive(ZipArchive archive, byte[] data, string fileName)
        {
            var entry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

            using (var entryStream = entry.Open())
            {
                entryStream.Write(data, 0, data.Length);
            }
        }

    }
}

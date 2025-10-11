using ECommerceAPI.Application.Services;
using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;

namespace ECommerceAPI.Infrastructure.Services;

public class FileService : IFileService
{
    readonly IWebHostEnvironment _webHostEnvironment;
    
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<(string fileName, string path)> datas = new();
        List<bool> results = new();
        
        foreach (IFormFile file in files)
        {
            string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
            bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
            
            // Dönen sonuçların true old. bilmemiz lazım. BU yüzden datas.Add() kullandık
            datas.Add((fileNewName, Path.Combine(path, fileNewName).Replace("\\", "/")));
            results.Add(result); 
        }

        if (results.TrueForAll(x => x.Equals(true)))
            return datas;
        
        return null;
        
        //todo Eğer ki yukarıdaki if geçerli değilse dosyaların sunucuda yüklenirken hata alındığna dair uyarıcı bir exception oluşturulup fırlatılması gerekiyor
        
    }

    async Task<string> FileRenameAsync(string path, string fileName, int suffix = 0)
    {
        
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedName = NameOperation.CharacterRegulatory(oldName);
            string newFileName = suffix == 0
                ? $"{regulatedName}{extension}"
                : $"{regulatedName}-{suffix}{extension}";

            if (File.Exists(Path.Combine(path, newFileName)))
                return await FileRenameAsync(path, fileName, suffix + 1);

            return newFileName;
        
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None,
                1024 * 1024, useAsync: false);

            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception ex)
        {
            //todo log!
            throw ex;
        }

    }
}
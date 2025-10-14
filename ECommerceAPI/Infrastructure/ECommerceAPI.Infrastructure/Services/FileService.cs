using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;

namespace ECommerceAPI.Infrastructure.Services;

public class FileService
{
    readonly IWebHostEnvironment _webHostEnvironment;
    readonly IStorageService _storageService;

    public FileService(IWebHostEnvironment webHostEnvironment, IStorageService storageService)
    {
        _webHostEnvironment = webHostEnvironment;
        _storageService = storageService;
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

    private async Task<string> FileRenameAsync(string uploadPath, string fileName, int suffix = 0)
    {
        string extension = Path.GetExtension(fileName);
        string oldName = Path.GetFileNameWithoutExtension(fileName);
        string regulatedName = NameOperation.CharacterRegulatory(oldName);
        string newFileName = suffix == 0
            ? $"{regulatedName}{extension}"
            : $"{regulatedName}-{suffix}{extension}";

        // uploadPath'i path olarak kullanabilmek için, web root'dan sonraki kısmı alıyoruz
        string relativePath = uploadPath.Replace(_webHostEnvironment.WebRootPath, "").TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        if (File.Exists(Path.Combine(uploadPath, newFileName)))
            return await FileRenameAsync(uploadPath, fileName, suffix + 1);
        else
            return newFileName;
    }
}
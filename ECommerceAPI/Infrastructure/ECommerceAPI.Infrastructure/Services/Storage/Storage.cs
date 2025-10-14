using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.Infrastructure.Services.Storage;

//Base class niyetinde oluşturduk
//Ortak olup davranış değiştirecek tüm memberlar IStorageInterface'inden implement ettirilip her birinde operasyon tek tek yaptırılacak
//Ama ortak olan memberlar (aynı algoritma kullanacak memberlar) Storage class'ına koyulacak, kalıtım ile alınacak
public class Storage
{
    protected delegate bool HasFileDelegate(string pathOrContainerName, string fileName);

    // Olan bir dosya ile aynı isimde dosya eklenirse sonuna 1,2,3,... ekleyerek isim oluşturur.
    protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName,
            HasFileDelegate hasFileMethod, int suffix = 0)
        // Dışardan erişilemesin sadece kalıtımsal olarak erişilsin diye protected yaptık
    {

        string extension = Path.GetExtension(fileName);
        string oldName = Path.GetFileNameWithoutExtension(fileName);
        string regulatedName = NameOperation.CharacterRegulatory(oldName);
        string newFileName = suffix == 0
            ? $"{regulatedName}{extension}"
            : $"{regulatedName}-{suffix}{extension}";

        //if (File.Exists(Path.Combine(path, newFileName)))
        if (hasFileMethod(pathOrContainerName, newFileName))
            return await FileRenameAsync(pathOrContainerName, fileName, hasFileMethod, suffix + 1);
        else
            return newFileName;

    }
    
}


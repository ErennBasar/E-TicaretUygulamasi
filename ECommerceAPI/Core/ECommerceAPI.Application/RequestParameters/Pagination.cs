namespace ECommerceAPI.Application.RequestParameters;

//Karşılayıcı bir RequestParameter nesnesi old. için class olmasına gerek yok.
//Struck , record olabilir 
public record Pagination
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
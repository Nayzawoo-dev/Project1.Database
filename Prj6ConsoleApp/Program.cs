// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
Console.WriteLine("This is Register Part");
Console.Write("Enter Your Name : ");
string Name = Console.ReadLine()!;
Console.Write("Enter Your Age : ");
int Age = Convert.ToInt32(Console.ReadLine());

HttpClient client = new HttpClient();
RegisterRequestModel model = new RegisterRequestModel()
{
    Name = Name,
    Age = Age
};
string Json = JsonConvert.SerializeObject(model);
var content = new StringContent(Json, Encoding.UTF8, Application.Json);
var item = client.PostAsync("https://localhost:7088/api/Register", content);
Console.WriteLine("This is Check Student Part");
Console.Write("Enter Your Roll No");
string RollNo = Console.ReadLine()!;
var item2 = client.GetAsync($"https://localhost:7088/api/StudentGet/{RollNo}");

if (item2.Result.IsSuccessStatusCode)
{
    string json = await item2.Result.Content.ReadAsStringAsync();
    Console.WriteLine(json);
}
Console.WriteLine("This Is Student Update Part");
Console.Write("Enter Your Roll No : ");
string RollNo1 = Console.ReadLine()!;
Console.Write("Update Your Age : ");
int Age1 = Convert.ToInt32( Console.ReadLine()!);
UpdateRequestModel model1 = new UpdateRequestModel
{
    RollNo = RollNo1,
    Age = Age1
};
string Json1 = JsonConvert.SerializeObject(model1);
var content1 = new StringContent(Json1, Encoding.UTF8, Application.Json);
var item3 = client.PatchAsync("https://localhost:7088/api/Update", content1);
await Task.WhenAll(item, item2,item3);

Console.ReadLine();





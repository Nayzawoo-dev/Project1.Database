// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Prj5ConsoleApp;
using RestSharp;
using static System.Net.Mime.MediaTypeNames;

Console.Write("Enter Your Roll No : ");
string RollNo = Console.ReadLine()!;
HttpClient client = new HttpClient();
//var res = await client.GetAsync($"https://localhost:7258/api/RollCallHistory/{RollNo}");
//if (res.IsSuccessStatusCode)
//{
//    string jsonStr = await res.Content.ReadAsStringAsync();
//    Console.WriteLine(jsonStr);
//}
RequestModel model = new RequestModel()
{
    RollNo = RollNo,
};
string json = JsonConvert.SerializeObject(model);
var content =new StringContent(json,Encoding.UTF8, Application.Json);
var res1 = client.PostAsync("https://localhost:7258/api/RollCallHistory/RollCallWithRollNo", content);
var res2 = client.GetAsync($"https://localhost:7258/api/RollCallHistory/{RollNo}");
await Task.WhenAll(res1,res2);
if (res1.Result.IsSuccessStatusCode)
{
    string jsonStr = await res1.Result.Content.ReadAsStringAsync();
    Console.WriteLine(jsonStr);
}

if (res2.Result.IsSuccessStatusCode)
{
    string jsonStr = await res2.Result.Content.ReadAsStringAsync();
    Console.WriteLine(jsonStr);
}

RestClient restClient = new RestClient();
RestRequest restRequest = new RestRequest($"https://localhost:7258/api/RollCallHistory/{RollNo}", RestSharp.Method.Get);
await restClient.ExecuteAsync(restRequest);

RestClient restClient1 = new RestClient();
RestRequest restRequest1 = new RestRequest("https://localhost:7258/api/RollCallHistory/RollCallWithRollNo",RestSharp.Method.Post);
restRequest1.AddJsonBody(model);
var response = await restClient1.ExecuteAsync(restRequest1);

if (response.IsSuccessStatusCode)
{
   string res = response.Content;
   Console.WriteLine(res);
}


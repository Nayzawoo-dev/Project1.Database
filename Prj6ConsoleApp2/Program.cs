using RestSharp;

Console.WriteLine("This is Register Part");
Console.Write("Enter Your Name : ");
string Name = Console.ReadLine()!;
Console.Write("Enter Your Age : ");
int Age = Convert.ToInt32(Console.ReadLine()!);
RequestModel model = new RequestModel()
{
    Name = Name,
    Age = Age
};
RestClient client = new RestClient();
RestRequest request = new RestRequest("https://localhost:7088/api/Register", RestSharp.Method.Post);
request.AddJsonBody(model);
await client.ExecuteAsync(request);

Console.WriteLine("This is Check Student Part");
Console.Write("Enter Your Roll No : ");
string RollNo = Console.ReadLine()!;
RestRequest request1 = new RestRequest($"https://localhost:7088/api/StudentGet/{RollNo}", RestSharp.Method.Get);
var response = await client.ExecuteAsync(request1);
if (response.IsSuccessStatusCode)
{
   var res = response.Content;
    Console.WriteLine(res);
}
Console.WriteLine("This is Delete Part");
Console.Write("Enter Your Roll No : ");
string RollNo1 = Console.ReadLine()!;
RestRequest request2 = new RestRequest($"https://localhost:7088/api/Delete/{RollNo1}", RestSharp.Method.Delete);
var response2 = await client.ExecuteAsync(request2);
if(response2.IsSuccessStatusCode)
{
    var res = response2.Content;
    Console.WriteLine(res);
}

Console.ReadLine();


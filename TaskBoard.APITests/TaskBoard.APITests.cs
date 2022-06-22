using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;


namespace TaskBoard.APITests
{
    public class ApiTests
    {
        private const string url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_ListAllTasks_CheckFirstTitle()
        {
            // Arrange
            this.request = new RestRequest(url);

            // Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].id, Is.EqualTo(1));
            Assert.That(tasks[0].title, Is.EqualTo("Project skeleton"));
        }

        [Test]
         public void Test_SearchTask_FindFirstResult()
         {
             // Arrange
             this.request = new RestRequest(url + "/search/{keyword}");
             request.AddUrlSegment("keyword", "Home Page");

             // Act
             var response = this.client.Execute(request, Method.Get);
             var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


             // Assert
             Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
             Assert.That(tasks.Count, Is.GreaterThan(0));
             Assert.That(tasks[0].title, Is.EqualTo("Home page"));
            
         }
        

         [Test]
         public void Test_SearchTasks_EmptyResults()
         {
             // Arrange
             this.request = new RestRequest(url + "/search/{keyword}");
             request.AddUrlSegment("keyword", "missing112233");

             // Act
             var response = this.client.Execute(request, Method.Get);
             var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


             // Assert
             Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
             Assert.That(tasks.Count, Is.EqualTo(0));
         }
        

         [Test]
         public void Test_CreateInvalidTask()
         {
             // Arrange
             this.request = new RestRequest(url);
             var body = new
             {
                 
                 description = "davaibirata",
                 board = "Open"
                 
             };
             request.AddJsonBody(body);

             // Act
             var response = this.client.Execute(request, Method.Post);

             // Assert
             Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
             Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
         }
        
         [Test]
         public void Test_CreateTask_ValidData()
         {
             // Arrange
             this.request = new RestRequest(url);
             var body = new
             {
                 title = "Misho" + DateTime.Now.Ticks,
                 description = "ABVBG" + DateTime.Now.Ticks,
                 board = "Open"
                
             };
             request.AddJsonBody(body);

             // Act
             var response = this.client.Execute(request, Method.Post);

             // Assert
             Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

             var allTasks = this.client.Execute(request, Method.Get);
             var tasks = JsonSerializer.Deserialize<List<Tasks>>(allTasks.Content);

             var lastContact = tasks.Last();


             Assert.That(lastContact.title, Is.EqualTo(body.title));
             Assert.That(lastContact.description, Is.EqualTo(body.description));

         }
    }
}

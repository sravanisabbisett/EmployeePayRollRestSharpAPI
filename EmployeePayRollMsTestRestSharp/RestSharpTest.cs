using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayRollMsTestRestSharp
{
    [TestClass]
    public class RestSharpTest
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/employees", Method.GET);
            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        ///It will return the no of employees in json file.
        /// </summary>
        [TestMethod]
        public void OnCallingGet_ShouldReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(14, dataResponse.Count);
            foreach(Employee employee in dataResponse)
            {
                Console.WriteLine("Id:" + employee.id + "\nName:" + employee.name + "\nSalary:" + employee.salary);
            }
        }

        /// <summary>
        /// Givens the employee on post should return added employee.
        /// </summary>
        [TestMethod]
        public void GivenEmployeeOnPost_ShouldReturnAddedEmployee()
        {
            //arrange
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Lipica");
            jObjectBody.Add("salary", "20000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            //deserialize the object employee
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Lipica", dataResponse.name);
            Assert.AreEqual("20000", dataResponse.salary);


        }

        [TestMethod]
        public void GienMultipleEmployeeOnPost_ShouldReturnName()
        {
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { name = "Salaija", salary = "10000000" });
            employeeList.Add(new Employee { name = "Badarinath", salary = "10000000" });
            foreach(Employee employee in employeeList)
            {
                RestRequest request = new RestRequest("/employees", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("name", employee.name);
                jObjectBody.Add("salary", employee.salary);
                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
                //act
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name,dataResponse.name);
            }
        }

        /// <summary>
        /// Updates the employee should return upadted salary.
        /// </summary>
        [TestMethod]
        public void UpdateTheEmployee_ShouldReturnUpadtedSalary()
        {
            //making a request for a particular employee to be updated
            RestRequest request = new RestRequest("/employees/8", Method.PUT);
            //creating a jobject for new data to be added in place of old
            //json represents a new json object
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "sravani");
            jObjectBody.Add("salary", "500000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(dataResponse.salary, "500000");

        }

        [TestMethod]
        public void DeleteEmployee_InEmployeePayRoll()
        {
            //making a request for a particular employee to be deleted.
            RestRequest request = new RestRequest("/employees/2",Method.DELETE);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode,HttpStatusCode.OK);
           
        }
    }
}

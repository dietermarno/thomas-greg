using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using thomasgregmvc.Models;

namespace thomasgregmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseUrl = string.Empty;
        string token = string.Empty;

        private string GetBaseUrl()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return $"{config.GetValue<string>("AppSettings:WebApiBaseUrl")}";
        }

        private User GetAuthenticationData()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            User data = new User()
            {
                Email = config.GetValue<string>("AppSettings:WebApiUsername"),
                Password = config.GetValue<string>("AppSettings:WebApiPassword")
            };
            return data;
        }

        private async Task<string> GetAuthentication()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage tokenResponse = await client.PostAsJsonAsync("api/Token", GetAuthenticationData());
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        return tokenResponse.Content.ReadAsStringAsync().Result;
                    }
                    return null;
                }
            }
        }

        private async Task<bool> CustomerExists(Customer customer)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Customers/CustomerExists", customer);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadFromJsonAsync<List<Customer>>().Result.Count > 0 ? true : false;
                    }
                    return true;
                }
            }
        }

        private async Task<Customer> GetCustomer(int id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync($"api/Customers/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadFromJsonAsync<Customer>().Result;
                    }
                    return new Customer();
                }
            }
        }

        private async Task<CustomerAddress> GetAddress(int id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync($"api/CustomersAddresses/GetAddress/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadFromJsonAsync<CustomerAddress>().Result;
                    }
                    return new CustomerAddress();
                }
            }
        }

        private async Task<List<CustomerAddress>> GetCustomerAddresses(int id)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync($"api/CustomersAddresses/GetCustomerAddresses/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadFromJsonAsync<List<CustomerAddress>>().Result;
                    }
                    return new List<CustomerAddress>();
                }
            }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            baseUrl = GetBaseUrl();
            token = GetAuthentication().Result.Replace("\"", "");
        }

        public async Task<IActionResult> Index()
        {
            List<Customer> customerData = new List<Customer>();
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync("api/Customers");
                    if (response.IsSuccessStatusCode)
                    {
                        customerData = response.Content.ReadFromJsonAsync<List<Customer>>().Result;
                    }
                    else
                    {
                        Error();
                    }
                    ViewData.Model = customerData;
                }
            }
            return View();
        }

        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            Customer newCustomer = new Customer()
            {
                Name = customer.Name,
                Email = customer.Email,
                Logo = customer.Logo
            };

            if (customer.Name != null)
            {
                if (string.IsNullOrEmpty(customer.Name))
                    ModelState.AddModelError("Name", "Please, fill the customer name.");

                if (string.IsNullOrEmpty(customer.Email))
                    ModelState.AddModelError("Email", "Plese, fill e-mail address.");

                if (await CustomerExists(customer))
                    ModelState.AddModelError("Email", "This e-mail is already in use.");

                if (ModelState.IsValid)
                {
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        using (var client = new HttpClient(httpClientHandler))
                        {
                            client.BaseAddress = new Uri(baseUrl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage response = await client.PostAsJsonAsync("api/Customers", newCustomer);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Error();
                            }
                            return View();
                        }
                    }
                }
                else
                {
                    return View(customer);
                }
            }
            return View();
        }

        public async Task<IActionResult> AddAddress(int id)
        {
            CustomerAddress newAddress = new CustomerAddress();
            newAddress = new CustomerAddress()
            {
                CustomerId = id,
                Street = null,
                Complement = null,
                ZipCode = null
            };
            return View(newAddress);
        }

        public async Task<IActionResult> AddAddressApply(CustomerAddress address)
        {
            if (address.Street != null)
            {
                if (ModelState.IsValid)
                {
                    CustomerAddress newAddress = new CustomerAddress()
                    {
                        Street = address.Street,
                        Number = address.Number,
                        Complement = address.Complement,
                        ZipCode = address.ZipCode,
                        CustomerId = address.CustomerId
                    };
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        using (var client = new HttpClient(httpClientHandler))
                        {
                            client.BaseAddress = new Uri(baseUrl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage response = await client.PostAsJsonAsync("api/CustomersAddresses", newAddress);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("UpdateCustomer", new { id = newAddress.CustomerId });
                            }
                            else
                            {
                                Error();
                            }
                            return View();
                        }
                    }
                }
            }
            return View(address);
        }

        public async Task<IActionResult> UpdateAddress(int id)
        {
            if (id != null)
            {
                CustomerAddress address = GetAddress(id).Result;
                return View(address);
            }
            return View();
        }

        public async Task<IActionResult> UpdateAddressApply(CustomerAddress address)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.PutAsJsonAsync($"api/CustomersAddresses/{address.Id}", address);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("UpdateCustomer", new { id = address.CustomerId });
                    }
                    else
                    {
                        Error();
                    }
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> UpdateCustomer(int id)
        {
            if (id != null)
            {
                Customer newCustomer = GetCustomer(id).Result;
                newCustomer.CustomerAddresses = GetCustomerAddresses(id).Result;
                return View(newCustomer);
            }
            return View();
        }

        public async Task<IActionResult> UpdateCustomerApply(Customer customer)
        {
            if (customer.Id != null)
            {
                if (string.IsNullOrEmpty(customer.Name))
                    ModelState.AddModelError("Name", "Please, fill the customer name.");

                if (string.IsNullOrEmpty(customer.Email))
                    ModelState.AddModelError("Email", "Plese, fill e-mail address.");

                if (await CustomerExists(customer))
                    ModelState.AddModelError("Email", "This e-mail is already in use.");

                if (ModelState.IsValid)
                {
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        using (var client = new HttpClient(httpClientHandler))
                        {
                            client.BaseAddress = new Uri(baseUrl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Customers/{customer.Id}", customer);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Error();
                            }
                            return View();
                        }
                    }
                }
                else
                {
                    return View(customer);
                }
            }
            return View();
        }

        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (id != null)
            {
                CustomerAddress address = GetAddress(id).Result;
                return View(address);
            }
            return View();
        }

        public async Task<IActionResult> DeleteAddressApply(CustomerAddress address)
        {
            if (address.Id != null)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.DeleteAsync($"api/CustomersAddresses/{address.Id}");
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("UpdateCustomer", new { id = address.CustomerId });
                        }
                        else
                        {
                            Error();
                        }
                        return View();
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (id != null)
            {
                Customer customer = GetCustomer(id).Result;
                customer.CustomerAddresses = GetCustomerAddresses(id).Result;
                return View(customer);
            }
            return View();
        }

        public async Task<IActionResult> DeleteCustomerApply(Customer customer)
        {
            if (customer.Id != null)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.DeleteAsync($"api/CustomersAddresses/DeleteAllAddresses/{customer.Id}");
                        if (response.IsSuccessStatusCode)
                        {
                            response = await client.DeleteAsync($"api/Customers/{customer.Id}");
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Error();
                            }
                        }
                        else
                        {
                            Error();
                        }
                        return View();
                    }
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
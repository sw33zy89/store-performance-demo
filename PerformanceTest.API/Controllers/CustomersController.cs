using Microsoft.AspNetCore.Mvc;
using PerformanceTesting.Core;
using PerformanceTesting.Core.Services;
using PerformanceTesting.Infrastructure.Persistence;

namespace PerformanceTest.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService) { 
            _customerService = customerService;
        }

        [HttpGet]
        [Route("id-sync/{id}")]
        public ActionResult GetById(int id)
        {
            Customer? customer = _customerService.GetCustomer(id);
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("id-sync-awaiter/{id}")]
        public ActionResult GetByIdAwaiter(int id)
        {
            Customer? customer = _customerService.GetCustomerAsync(id).GetAwaiter().GetResult();
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("id-split-sync/{id}")]
        public ActionResult GetByIdSplit(int id)
        {
            Customer? customer = _customerService.GetCustomer(id, true);
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("id-split-sync-awaiter/{id}")]
        public ActionResult GetByIdAwaiterSplit(int id)
        {
            Customer? customer = _customerService.GetCustomerAsync(id, true).GetAwaiter().GetResult();
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("id-async/{id}")]
        public async Task<ActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerService.GetCustomerAsync(id, false, cancellationToken);
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("id-split-async/{id}")]
        public async Task<ActionResult> GetByIdSplitAsync(int id, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerService.GetCustomerAsync(id, true, cancellationToken);
            return Ok(customer?.Id);
        }

        [HttpGet]
        [Route("batch-sync/{num}")]
        public ActionResult GetBatch(int num)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++) {
                ids.Add(num);
            }
            List<Customer> customers = _customerService.GetCustomers(ids, false);

            return Ok(customers.Count);
        }

        [HttpGet]
        [Route("batch-sync-awaiter/{num}")]
        public ActionResult GetBatchAwaiter(int num)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ids.Add(num);
            }
            List<Customer> customers = _customerService.GetCustomersAsync(ids, false).GetAwaiter().GetResult();

            return Ok(customers.Count);
        }

        [HttpGet]
        [Route("batch-split-sync/{num}")]
        public ActionResult GetBatchSplit(int num)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ids.Add(num);
            }
            List<Customer> customers = _customerService.GetCustomers(ids, true);

            return Ok(customers.Count);
        }

        [HttpGet]
        [Route("batch-split-sync-awaiter/{num}")]
        public ActionResult GetBatchSplitAwaiter(int num)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ids.Add(num);
            }
            List<Customer> customers = _customerService.GetCustomersAsync(ids, true).GetAwaiter().GetResult();

            return Ok(customers.Count);
        }

        [HttpGet]
        [Route("batch-async/{num}")]
        public async Task<ActionResult> GetBatchAsync(int num, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ids.Add(num);
            }
            List<Customer> customers = await _customerService.GetCustomersAsync(ids, false, cancellationToken);

            return Ok(customers.Count);
        }

        [HttpGet]
        [Route("batch-async-split/{num}")]
        public async Task<ActionResult> GetBatchSplitAsync(int num, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ids.Add(num);
            }
            List<Customer> customers = await _customerService.GetCustomersAsync(ids, true, cancellationToken);

            return Ok(customers.Count);
        }

        [HttpPost]
        [Route("insert-sync/{num}")]
        public ActionResult InsertRandomSync(int num)
        {
            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < num; i++)
            {
                Customer customer = DataGenerator.GenerateRandomCustomer();
                _customerService.Insert(customer);
            }

            return Ok(customers.Count);
        }

        [HttpPost]
        [Route("insert-sync-awaiter/{num}")]
        public ActionResult InsertRandomSyncAwaiter(int num, CancellationToken cancellationToken)
        {
            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < num; i++)
            {
                Customer customer = DataGenerator.GenerateRandomCustomer();
                _customerService.InsertAsync(customer, cancellationToken).GetAwaiter().GetResult();

                customers.Add(customer);
            }

            return Ok(customers.Count);
        }

        [HttpPost]
        [Route("insert-async/{num}")]
        public async Task<ActionResult> InsertRandomAsync(int num, CancellationToken cancellationToken)
        {
            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < num; i++)
            {
                Customer customer = DataGenerator.GenerateRandomCustomer();
                await _customerService.InsertAsync(customer, cancellationToken);

                customers.Add(customer);
            }

            return Ok(customers.Count);
        }

        [HttpPost]
        [Route("insert-async-whenall/{num}")]
        public async Task<ActionResult> InsertRandomAsyncWhenAll(int num, CancellationToken cancellationToken)
        {
            List<Customer> customers = new List<Customer>();
            List<Task<Customer>> tasks = new List<Task<Customer>>();
            for (int i = 0; i < num; i++)
            {
                Customer customer = DataGenerator.GenerateRandomCustomer();
                tasks.Add(_customerService.InsertAsync(customer, cancellationToken));
            }

            customers.AddRange(await Task.WhenAll(tasks));

            return Ok(customers.Count);
        }
    }
}

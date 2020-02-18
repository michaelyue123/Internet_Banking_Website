using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IBW.Model;
using WebApi.Models.DataManager;

namespace WebApi.Controllers
{
    [Route("api/BillPays")]
    [ApiController]
    public class BillPaysController : ControllerBase
    {
        private readonly BillPayManager _repo;

        public BillPaysController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/BillPays
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/BillPays/5
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        // PUT: api/BillPays/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public void Put([FromBody] BillPay billPay)
        {
            _repo.Update(billPay.BillPayID, billPay);
        }

        // POST: api/BillPays
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public void Post([FromBody] BillPay billPay)
        {
            _repo.Add(billPay);
        }

        // DELETE: api/BillPays/5
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IBW.Model;
using WebApi.Models.DataManager;

namespace WebApi.Controllers
{
    [Route("api/Payees")]
    [ApiController]
    public class PayeesController : ControllerBase
    {
        private readonly PayeeManager _repo;

        public PayeesController(PayeeManager repo)
        {
            _repo = repo;
        }

        // GET: api/Payees
        [HttpGet]
        public IEnumerable<Payee> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Payees/5
        [HttpGet("{id}")]
        public Payee Get(int id)
        {
            return _repo.Get(id);
        }

        // PUT: api/Payees/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public void Put([FromBody] Payee payee)
        {
            _repo.Update(payee.PayeeID, payee);
        }

        // POST: api/Payees
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public void Post([FromBody] Payee payee)
        {
            _repo.Add(payee);
        }

        // DELETE: api/Payees/5
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}

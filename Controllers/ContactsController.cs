using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ContactsController : ControllerBase
    {      
        
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]        
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }
        
        [HttpGet]
        [Route("Find/{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact!=null)
            {
                return Ok(contact);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact=new Contact()
            {
                Id=Guid.NewGuid(),
                Address=addContactRequest.Address,
                Email=addContactRequest.Email,
                FirstName=addContactRequest.FirstName,
                Phone=addContactRequest.Phone
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id,UpdateContactRequest updateContactRequest)
        {
            var contact =await dbContext.Contacts.FindAsync(id);
            if(contact!=null)
            {
                contact.FirstName=updateContactRequest.FirstName;
                contact.Address=updateContactRequest.Address;
                contact.Email=updateContactRequest.Email;
                contact.Phone=updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();
                return Ok(contact);

            }
            return NotFound();
        }
    }
}
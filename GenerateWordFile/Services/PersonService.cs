using AutoMapper;
using GenerateWordFile.Data;
using GenerateWordFile.Helpers;
using GenerateWordFile.Models;
using GenerateWordFile.ViewModels;
using Microsoft.EntityFrameworkCore;

public interface IPersonService
{
    Task<Person> CreatePersonAsync(PersonVM personViewModel);
    Task<bool> UpdatePersonAsync(int id, PersonUpdateVM personViewModel);
    Task<Person> GetPersonByIdAsync(int id);
}

public class PersonService : IPersonService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PersonService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Person> CreatePersonAsync(PersonVM personVM)
    {
        if (!ValidationHelper.IsValidEmail(personVM.Email))
            throw new ArgumentException("Invalid email format.");
        personVM.FinCode = GenerateFinCode();
        personVM.CardNumber = GenerateCardNumber();
        if (!_context.People.Any(p => p.EmailAddress == personVM.Email) &&
            !_context.People.Any(p => p.FinCode == personVM.FinCode) &&
            !_context.People.Any(p => p.CardNumber == personVM.CardNumber))
        {
            Person person = _mapper.Map<Person>(personVM);
            person.FinCode = personVM.FinCode;
            person.CardNumber = personVM.CardNumber ;

            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }
        else
        {
            throw new ArgumentException("Email, FinCode, or CardNumber already exists.");
        }
    }

    public async Task<bool> UpdatePersonAsync(int id, PersonUpdateVM personVM)
    {
        if (!id.PersonExists(_context)) return false;
        Person person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
        if (person == null)
        {
            return false;
        }
        // Person
        if(personVM.FirstName !=null && personVM.FirstName!=person.FirstName) person.FirstName = personVM.FirstName.Capitalize();
        if(personVM.LastName !=null && personVM.LastName!=person.LastName) person.FirstName = personVM.LastName.Capitalize();
        if(personVM.FatherName !=null && personVM.FatherName!=person.FatherName) person.FatherName = personVM.FatherName.Capitalize();
        if (!await _context.People.AnyAsync(p => p.EmailAddress == personVM.Email && p.Id != id) && ValidationHelper.IsValidEmail(personVM.Email) && personVM.Email!=person.EmailAddress) 
        person.EmailAddress = personVM.Email;
        if(personVM.Birthdate!= person.Birthday) person.Birthday = personVM.Birthdate;
        if(personVM.gender!=null && personVM.gender.ToString()!=person.Gender)person.Gender = personVM.gender.ToString();

        // Xüsusi xüsusiyyətləri dəyişdirməməyə diqqət yetir
        _context.Entry(person).Property(p => p.Id).IsModified = false;
        _context.Entry(person).Property(p => p.FinCode).IsModified = false;
        _context.Entry(person).Property(p => p.CardNumber).IsModified = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await _context.People.FindAsync(id);
    }

    private string GenerateFinCode()
    {
        string finCode;
        do
        {
            // Generate a random 7-character alphanumeric FinCode
            finCode = GenerateRandomString(7);
        } while (_context.People.Any(p => p.FinCode == finCode));

        return finCode;
    }

    private string GenerateCardNumber()
    {
        string cardNumber;
        do
        {
            // Generate a random 9-character alphanumeric SerialNumber
            cardNumber = GenerateRandomString(2)+ GenerateRandomNumber(7); ;
        } while (_context.People.Any(p => p.CardNumber == cardNumber));

        return cardNumber;
        
    }
   
    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string GenerateRandomNumber(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
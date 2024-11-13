using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ComedyDbContext _context;

    public UserController(ComedyDbContext context)
    {
        _context = context;
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        // Implementacja weryfikacji hasła, np. porównanie hasła z hashem (użyj BCrypt lub SHA256)
        return password == passwordHash;  // Przykład - porównujemy hasło bez hashowania (należy dodać właściwą weryfikację)
    }

    // Logowanie użytkownika
    [HttpPost("login")]
    public async Task<ActionResult<UserResponseDto>> LoginUser(UserLoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized("Nieprawidłowy adres e-mail lub hasło.");
        }

        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized("Nieprawidłowy adres e-mail lub hasło.");
        }

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            SocialLogins = await _context.SocialLogins
                .Where(sl => sl.UserId == user.Id)
                .Select(sl => new SocialLoginDto { Provider = sl.Provider, ProviderUserId = sl.ProviderUserId })
                .ToListAsync()
        });
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
                EducationLevel = u.EducationLevel
            })
            .ToListAsync();

        return Ok(users);
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> RegisterUser(UserRegistrationDto registrationDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registrationDto.Email))
        {
            return BadRequest("Użytkownik z podanym adresem e-mail już istnieje.");
        }

        var user = new User
        {
            Username = registrationDto.Username,
            Email = registrationDto.Email,
            PasswordHash = HashPassword(registrationDto.Password)  // Hashowanie hasła
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            SocialLogins = new List<SocialLoginDto>()
        });
    }

    // Logowanie przez platformy społecznościowe
    [HttpPost("social-login")]
    public async Task<ActionResult<UserResponseDto>> SocialLogin(SocialLoginDto socialLoginDto)
    {
        var socialLogin = await _context.SocialLogins
            .Include(sl => sl.User)
            .FirstOrDefaultAsync(sl => sl.Provider == socialLoginDto.Provider && sl.ProviderUserId == socialLoginDto.ProviderUserId);

        if (socialLogin == null)
        {
            return NotFound("Użytkownik nie jest połączony z podaną platformą logowania.");
        }

        var user = socialLogin.User;

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            SocialLogins = await _context.SocialLogins
                .Where(sl => sl.UserId == user.Id)
                .Select(sl => new SocialLoginDto { Provider = sl.Provider, ProviderUserId = sl.ProviderUserId })
                .ToListAsync()
        });
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            EducationLevel = user.EducationLevel
        });
    }


    // GET: api/User/{userId}/jokes
    [HttpGet("{userId}/jokes")]
    public async Task<ActionResult<IEnumerable<JokeDto>>> GetUserJokes(int userId)
    {
        // Sprawdzenie, czy użytkownik istnieje
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return NotFound("Użytkownik nie istnieje.");
        }

        // Pobranie listy żartów użytkownika
        var jokes = await _context.Jokes
            .Where(j => j.UserId == userId)
            .Select(j => new JokeDto
            {
                JokeId = j.Id,
                Text = j.Text,
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();

        return Ok(jokes);
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            DateOfBirth = userDto.DateOfBirth,
            EducationLevel = userDto.EducationLevel
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        userDto.Id = user.Id;
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Username = userDto.Username;
        user.Email = userDto.Email;
        user.DateOfBirth = userDto.DateOfBirth;
        user.EducationLevel = userDto.EducationLevel;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    // Funkcja pomocnicza do hashowania hasła (przykładowa implementacja)
    private string HashPassword(string password)
    {
        // Implementacja hashowania hasła (np. użycie BCrypt lub SHA256)
        return password;  // Przykładowo zwracamy hasło bez hashowania (należy dodać właściwe hashowanie)
    }
    // Other CRUD operations...
}

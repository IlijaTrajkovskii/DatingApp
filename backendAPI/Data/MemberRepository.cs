using backendAPI.Entities;
using backendAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backendAPI.Data;

// implementacijata na IMemberRepository
public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return await context.Members.ToListAsync();
    }

    // naogjame slikite na memberot
    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        return await context.Members
            .Where(x=> x.Id == memberId)
            .SelectMany(x=> x.Photos)  // selektirame samo potrebnite koloni
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }
}
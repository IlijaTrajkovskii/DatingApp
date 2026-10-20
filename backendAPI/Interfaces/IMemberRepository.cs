using backendAPI.Entities;

namespace backendAPI.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    
    // cim vrakjame task znaci funkcijata ke bide ASYNC
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);

    Task<Member?> GetMemberForUpdate(string id);


}
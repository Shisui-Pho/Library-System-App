using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories;

public class MessageRequestRepository(AppDBContext dbcontext) 
    : BaseRepository<MessageRequest>(dbcontext),  
      IMessageReuqestsRepository
{

}

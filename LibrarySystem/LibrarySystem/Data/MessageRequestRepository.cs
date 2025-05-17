using LibrarySystem.Models;

namespace LibrarySystem.Data;

public class MessageRequestRepository(AppDBContext dbcontext) 
    : BaseRepository<MessageRequest>(dbcontext),  
      IMessageReuqestsRepository
{

}

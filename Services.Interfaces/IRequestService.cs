using Database.Models;
using System;
using System.Linq;

namespace Bizico_Project
{
    public interface IRequestService
    {
        IQueryable<Request> GetRequests();
        IQueryable<Request> GetRequests(int page, int size, string sorting);
        Request GetRequestById(Guid id);
        Request CreateRequest(Request request);
        IQueryable<Request> GetMyRequests();
        IQueryable<Request> GetRequestByUserId(Guid id);
        Request UpdateRequest(Request request);
        void Delete(Guid id);
    }
}
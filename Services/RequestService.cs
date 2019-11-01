using Bizico_Project;
using Database;
using Database.Models;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Services
{
    public class RequestService : IRequestService
    {
        private readonly FreelanceContext _requestcontext;
        private readonly CurrentUserInfo _info;

        public RequestService(FreelanceContext requestcontext, CurrentUserInfo info)
        {
            _requestcontext = requestcontext;
            _info = info;
        }

        public IQueryable<Request> GetRequests()
        {
            return _requestcontext.Requests;
        }

        public IQueryable<Request> GetRequests(int page, int size, string sorting)
        {
            var allRequests = _requestcontext.Requests;
            var data = allRequests.AsQueryable();
            data = SortService.ApplySort(data, sorting);

            return data.Skip(size * (page - 1)).Take(size);
        }


        public Request GetRequestById(Guid id)
        {
            var request = GetRequests().Single(x => x.Id == id);
            return request;
        }

        public IQueryable<Request> GetRequestByUserId(Guid id)
        {
            var request = GetRequests().Where(x => x.UserId == id);
            return request;
        }

        public IQueryable<Request> GetMyRequests()
        {
            var request = GetRequests().Where(x => x.UserId.ToString() == _info.Id);
            return request;
        }

        public Request CreateRequest(Request request)
        {
            request.UserId = Guid.Parse(_info.Id);
            request.Date = DateTime.Now;
            _requestcontext.Requests.Add(request);
            _requestcontext.SaveChanges();

            return request;
        }
         
        public Request UpdateRequest(Request request)
        {
            request.UserId = Guid.Parse(_info.Id);
            var userid = _requestcontext.Requests.First(x => x.Id == request.Id).UserId;
            if (userid.ToString() == _info.Id)
            {
                var lst = _requestcontext.ChangeTracker.Entries().ElementAt(0).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                _requestcontext.Attach(request);
                _requestcontext.Entry(request).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _requestcontext.SaveChanges();
            }

            return request;
        }

        public void Delete(Guid Id)
        {
            if (_info.Role == "Client")
            {
                var userid = _requestcontext.Requests.First(x => x.Id == Id).UserId;
                if (userid.ToString() == _info.Id)
                {
                    _requestcontext.Remove(GetRequestById(Id));
                    _requestcontext.SaveChanges();
                }
            }
            else
            {
                _requestcontext.Remove(GetRequestById(Id));
                _requestcontext.SaveChanges();
            }
        }
    }
}

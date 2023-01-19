using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Reenbit.HireMe.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public JobService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
        public async Task<List<JobView>> GetApprovedJob()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobViewRepository>();
                var result = await jobRepository.GetApproved();

                return result.Select(MapJobViewToGridDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }

        }        

        public async Task AddJob(JobDTO createJob, string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();

                var newJob = new Job
                {
                    Email = email,
                    AboutProject = createJob.AboutProject,
                    Benefits = createJob.Benefits,
                    CompanyName = createJob.CompanyName,
                    Contacts = createJob.Contacts,
                    EmploymentType = createJob.EmploymentType,
                    EnglishLevel = createJob.EnglishLevel,
                    JobRequirements = createJob.JobRequirements,
                    JobTitle = createJob.JobTitle,
                    SalaryRange = createJob.SalaryRange,
                    Stack = createJob.Stack,
                    StagesInterview = createJob.StagesInterview,
                    WorkplaceType = createJob.WorkplaceType,
                    ContactLink = createJob.ContactLink,
                    ContactType = createJob.ContactType,
                    Tags = createJob.Tags,
                    IsApproved = false,
                    DateCreated = DateTime.UtcNow.ToString("dd.MM.yyyy"),
                    City = createJob.City,
                    Country = createJob.Country,
                    Experience = createJob.Experience
                };

                jobRepository.Add(newJob);

                await uow.SaveChangesAsync();
                this.MailNotifModerator(createJob.JobTitle.ToString());
            }
        }

        public async Task<List<Job>> GetJobsByUserEmail(string email)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();
                var result = await jobRepository.GetJobsByEmail(email);

                return result.Select(MapJobToGridDTO)
                             .OrderByDescending(u => u.Id)
                             .ToList();
            }
        }

        public async Task DeleteJob(int id)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();

                var result = await jobRepository.FindAsync(c => c.Id == id);
                if (result.Count > 0)
                {
                    jobRepository.Remove(result.First());
                }

                await uow.SaveChangesAsync();
            }
        }

        public async Task UpdateJob(Job job)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();

                var newJob = new Job
                {
                    Id = job.Id,
                    AboutProject = job.AboutProject,
                    Benefits = job.Benefits,
                    CompanyName = job.CompanyName,
                    Contacts = job.Contacts,
                    EmploymentType = job.EmploymentType,
                    EnglishLevel = job.EnglishLevel,
                    JobRequirements = job.JobRequirements,
                    JobTitle = job.JobTitle,
                    SalaryRange = job.SalaryRange,
                    Stack = job.Stack,
                    StagesInterview = job.StagesInterview,
                    WorkplaceType = job.WorkplaceType,
                    ContactLink = job.ContactLink,
                    ContactType = job.ContactType,
                    Tags = job.Tags,
                    IsApproved = false,
                    DateCreated = job.DateCreated,
                    Country = job.Country,
                    City = job.City,
                    Experience = job.Experience
                };

                jobRepository.Update(newJob);
                await uow.SaveChangesAsync();
            }
        }

        public async Task<List<Job>> GetAllJob()
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();
                var result = await jobRepository.GetAllJobs();

                return result.Select(MapJobToGridDTO)
                             .OrderBy(u => u.IsApproved)
                             .ThenByDescending(u => u.DateCreated)
                             .ToList();
            }

        }

        public async Task UpdateJobModerator(Job job)
        {
            string emailJob = "";
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var jobRepository = uow.GetRepository<IJobRepository>();
                emailJob = jobRepository.GetJobEmailById(job.Id).Result;

                var newJob = new Job
                {
                    Id = job.Id,
                    AboutProject = job.AboutProject,
                    Benefits = job.Benefits,
                    CompanyName = job.CompanyName,
                    Contacts = job.Contacts,
                    EmploymentType = job.EmploymentType,
                    EnglishLevel = job.EnglishLevel,
                    JobRequirements = job.JobRequirements,
                    JobTitle = job.JobTitle,
                    SalaryRange = job.SalaryRange,
                    Stack = job.Stack,
                    StagesInterview = job.StagesInterview,
                    WorkplaceType = job.WorkplaceType,
                    ContactLink = job.ContactLink,
                    ContactType = job.ContactType,
                    Tags = job.Tags,
                    IsApproved = job.IsApproved,
                    DateCreated = job.DateCreated,
                    Email = emailJob,
                    Country = job.Country,
                    City = job.City,
                    Experience = job.Experience
                };

                jobRepository.Update(newJob);
                await uow.SaveChangesAsync();
            }
            this.MailNotifRecruter(job.JobTitle.ToString(), emailJob.ToString(), job.IsApproved);
        }


        private Job MapJobToGridDTO(Job job)
        {
            return job;
        }
        private JobView MapJobViewToGridDTO(JobView job)
        {
            return job;
        }

        private void MailNotifModerator(string nameJob)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rozrobnyk", "rozrobnyk@devorld.studio"));
            message.To.Add(new MailboxAddress("Andrii", "andrii@dontpanic.team"));
            message.To.Add(new MailboxAddress("Anastasiia", "anastasia.iskandarova@gmail.com"));
            message.Subject = "New job";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey,
New vacancy on the Rozrobnyk. Title: " + nameJob + @"
Go to the website to view details and approve: https://rozrobnyk.com/ ('Moderator Page' оn the your profile page)"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("mail.adm.tools", 465, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("rozrobnyk@devorld.studio", "-g;9y4CeCZ^6");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private void MailNotifRecruter(string nameJob, string email, bool approve)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rozrobnyk", "rozrobnyk@devorld.studio"));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Update your vacancy";

            if(approve)
            {
                message.Body = new TextPart("plain")
                {
                    Text = @"Hey,
Moderators have approved your vacancy '" + nameJob + @"'. It's now in the Job List: https://rozrobnyk.com/jobs

Keep in touch 🤝 
https://rozrobnyk.com/ "
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("mail.adm.tools", 465, true);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("rozrobnyk@devorld.studio", "-g;9y4CeCZ^6");

                    client.Send(message);
                    client.Disconnect(true);
                }
            } 
        }
    }
}

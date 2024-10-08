using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Enums;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class CourseRequestConfiguration : IEntityTypeConfiguration<CourseRequest>
    {
        public void Configure(EntityTypeBuilder<CourseRequest> builder)
        {
            builder
                .HasOne(cr => cr.Child)
                .WithMany(c => c.CourseRequests)
                .HasForeignKey(cr => cr.ChildId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CourseRequests)
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            List<CourseRequest> list = new List<CourseRequest>
            {
                new CourseRequest
                {
                    Id = 1,
                    CourseId = 1,
                    ChildId = 1,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 2,
                    CourseId = 1,
                    ChildId = 2,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 3,
                    CourseId = 1,
                    ChildId = 3,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 4,
                    CourseId = 1,
                    ChildId = 4,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 5,
                    CourseId = 2,
                    ChildId = 2,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 6,
                    CourseId = 2,
                    ChildId = 5,
                    Status = RequestStatus.Accepted
                },
                new CourseRequest
                {
                    Id = 7,
                    CourseId = 3,
                    ChildId = 5,
                    Status = RequestStatus.Rejected
                },
                new CourseRequest
                {
                    Id = 8,
                    CourseId = 4,
                    ChildId = 5,
                    Status = RequestStatus.Pending
                },
            };

            builder.HasData(list);
        }
    }
}

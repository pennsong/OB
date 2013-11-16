using System;
using System.Linq;
using FrameLog.Contexts;
using FrameLog.Models;
using OB.Models.DAL;

namespace OB.Models.FrameLog
{
    public class ExampleContextAdapter : DbContextAdapter<ChangeSet, User>
    {
        private OBContext context;

        public ExampleContextAdapter(OBContext context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<User>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<User>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<User>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }

        public override Type UnderlyingContextType
        {
            get { return typeof(OBContext); }
        }
    }
}

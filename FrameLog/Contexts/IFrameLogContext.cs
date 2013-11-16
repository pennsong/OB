using System;
using System.Data;
using System.Data.Objects;
using FrameLog.Models;
using System.Data.Metadata.Edm;

namespace FrameLog.Contexts
{
    public interface IFrameLogContext<TChangeSet, TPrincipal> : IHistoryContext<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        int SaveChanges(SaveOptions options);
        ObjectStateManager ObjectStateManager { get; }
        void AcceptAllChanges();

        object GetObjectByKey(EntityKey key);
        void AddChangeSet(TChangeSet changeSet);

        Type UnderlyingContextType { get; }
        MetadataWorkspace Workspace { get; }
    }
}

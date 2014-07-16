using System;
using System.Transactions;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;
using Novaweb.EventTest.WebApi.Commands;

namespace Novaweb.EventTest.WebApi
{
    public static class EventsStoreConfig
    {
        private static readonly Guid StreamId = Guid.NewGuid();
        //private static readonly Guid StreamId = new Guid("CAEA50AC-B917-44EB-80E4-AB67C78C485F");
        private static readonly byte[] EncryptionKey =
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };
        private static IStoreEvents store;

        public static void Initialize()
        {
            //using (var scope = new TransactionScope())
            //{
                store = WireupEventStore();
                using (var stream = store.CreateStream(StreamId))
                {
                    stream.Add(new MeetingScheduled() { MeetingName = "Test meeting" });
                    stream.CommitChanges(Guid.NewGuid());
                }
                store.Dispose();
            //}

        }

        private static IStoreEvents WireupEventStore()
        {
            return Wireup.Init()
                         .LogToOutputWindow()
                         //.UsingInMemoryPersistence()
                         .UsingSqlPersistence("EventStore") // Connection string is in app.config
                         .WithDialect(new MsSqlDialect())
                         //.EnlistInAmbientTransaction() // two-phase commit
                         .InitializeStorageEngine()
                         //.TrackPerformanceInstance("example")
                         //.UsingJsonSerialization()
                         //.Compress()
                         //.EncryptWith(EncryptionKey)
                         //.HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
                         //.UsingSynchronousDispatchScheduler()
                         //.DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
                         .Build();
        }

        private static void DispatchCommit(ICommit commit)
        {
            // This is where we'd hook into our messaging infrastructure, such as NServiceBus,
            // MassTransit, WCF, or some other communications infrastructure.
            // This can be a class as well--just implement IDispatchCommits.
            try
            {
                foreach (EventMessage @event in commit.Events)
                {
                    //Console.WriteLine(Resources.MessagesDispatched + ((SomeDomainEvent)@event.Body).Value);
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(Resources.UnableToDispatch);
            }
        }

    }
}
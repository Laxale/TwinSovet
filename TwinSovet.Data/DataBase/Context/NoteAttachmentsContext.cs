﻿using System.Data.Entity;

using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Context 
{
    public class NoteAttachmentsContext : CommonAttachmentContext<NoteAttachmentModel> 
    {
        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuilder, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        /// <param name="modelBuilder"> The builder that defines the model for the context being created. </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new NotesConfiguration());
            //modelBuilder.Configurations.Add(new ChildAttachmentDescriptorsConfiguration<NoteAttachmentModel>());

            modelBuilder.Entity<OfNoteAttachmentDescriptor>()
                .HasRequired(descriptor => descriptor.NavigationParent)
                .WithMany(note => note.ChildAttachmentDescriptors)
                .HasForeignKey(descriptor => descriptor.ParentId)
                .WillCascadeOnDelete(true);

            CreateTable(modelBuilder);
        }
    }
}
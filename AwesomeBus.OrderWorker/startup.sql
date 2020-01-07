if object_id('orderworker.SubmittedOrder', 'U') is null
    create table orderworker.SubmittedOrder (
        Id uniqueidentifier not null primary key,
        Value int not null
    )
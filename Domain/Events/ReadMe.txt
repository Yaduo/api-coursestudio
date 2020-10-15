### Note!!! Never subscribe ServiceEvent in domain layer 
# Internal Events
# Events in Domain.Events are ACID
# Only apply within one transaction (befor database commit)
# Subscribe and Handled by DomainEventHandler for a repository
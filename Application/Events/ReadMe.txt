### Note!!! Never subscribe DomainEvent in application layer 
# External Events
# Events in Service.Events might not ACID
# Might apply outside one transaction (after database commit)
# Subscribe and Handled by EventHandler for application services

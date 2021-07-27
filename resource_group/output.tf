output "resource_group_name" {
    value = resource_group_name.rg.name
    description = "Resource Group Name"
}

output "location" {
    value = resource_group_name.rg.location
    description = "Resource Group Location"
}
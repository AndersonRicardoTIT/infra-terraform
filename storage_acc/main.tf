resource "azurerm_storage_account" "storageacc01" {
  name                     = "storageacc01"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "GRS"

  tags = {
    environment = var.environment
  }
}

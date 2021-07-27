resource "azurerm_network_security_group" "vnetsecg01" {
  name                = "VnetSecurityGroup01"
  location            = var.location
  resource_group_name = var.resource_group_name
}

resource "azurerm_network_ddos_protection_plan" "vnetddosprot01" {
  name                = "VnetDdosplan01"
  location            = var.location
  resource_group_name = var.resource_group_name
}

resource "azurerm_virtual_network" "vnet01" {
  name                = "virtualNetwork01"
  location            = var.location
  resource_group_name = var.resource_group_name
  address_space       = ["10.0.0.0/24"]
  dns_servers         = ["10.0.0.4", "10.0.0.5"]

  ddos_protection_plan {
    id     = azurerm_network_ddos_protection_plan.vnetddosprot01.id
    enable = true
  }

  subnet {
    name           = "vnetsubnet01"
    address_prefix = "10.0.0.0/24"
    security_group = azurerm_network_security_group.vnetsecg01.id
  }

  tags = {
    environment = var.environment
  }
}
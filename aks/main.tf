resource "azurerm_kubernetes_cluster" "clusteraks" {
  name                = "clusteraks01_${var.environment}"
  location            = var.resource_group_name
  resource_group_name = var.location
  dns_prefix          = "exampleaks1"

  default_node_pool {
    name       = "default"
    node_count = 1
    vm_size    = "Standard_D2_v2"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environment = var.environment
  }
}
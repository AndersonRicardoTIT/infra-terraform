output "client_certificate" {
  value = azurerm_kubernetes_cluster.clusteraks.kube_config.0.client_certificate
}

output "kube_config" {
  value = azurerm_kubernetes_cluster.clusteraks.kube_config_raw
}
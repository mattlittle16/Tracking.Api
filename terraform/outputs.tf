# ECR Repository Outputs
output "ecr_repository_url" {
  description = "URL of the ECR repository"
  value       = module.ecr.repository_url
}

output "ecr_repository_arn" {
  description = "ARN of the ECR repository"
  value       = module.ecr.repository_arn
}

output "container_image_uri" {
  description = "Full container image URI including tag"
  value       = "${module.ecr.repository_url}:${var.image_tag}"
}

# SSL Certificate Outputs
output "certificate_arn" {
  description = "ARN of the SSL certificate"
  value       = module.ssl_certificate.certificate_arn
}

output "certificate_domain_validation_options" {
  description = "Domain validation options - add these DNS records to Route53"
  value       = module.ssl_certificate.domain_validation_options
}

output "domain_setup_instructions" {
  description = "Instructions for completing domain setup"
  value = <<-EOT
    1. Apply this Terraform configuration
    2. Check the certificate_domain_validation_options output
    3. Add the validation DNS records to your Route53 hosted zone
    4. Wait for certificate validation (up to 30 minutes)
    5. Your app will be available at https://${var.domain_name}
  EOT
}

# Lightsail Container Service Outputs
output "container_service_arn" {
  description = "ARN of the Lightsail container service"
  value       = module.lightsail_container.service_arn
}

output "container_service_url" {
  description = "Default URL of the Lightsail container service"
  value       = module.lightsail_container.service_url
}

output "container_service_private_domain" {
  description = "Private domain name of the Lightsail container service"
  value       = module.lightsail_container.private_domain_name
}

# DNS Outputs
output "dns_record_name" {
  description = "DNS record name for the Tracking API"
  value       = module.dns.record_name
}

output "dns_record_fqdn" {
  description = "Fully qualified domain name for the Tracking API"
  value       = module.dns.record_fqdn
}

output "lightsail_hostname" {
  description = "Lightsail container service hostname"
  value       = module.dns.lightsail_hostname
}

# Application URLs
output "api_url" {
  description = "Public URL for the Tracking API"
  value       = "https://${var.domain_name}"
}

output "swagger_url" {
  description = "Swagger UI endpoint URL"
  value       = "https://${var.domain_name}/swagger"
}

output "service_name" {
  description = "Name of the Lightsail container service"
  value       = aws_lightsail_container_service.tracking_api.name
}

output "service_arn" {
  description = "ARN of the Lightsail container service"
  value       = aws_lightsail_container_service.tracking_api.arn
}

output "service_url" {
  description = "Default URL of the container service"
  value       = aws_lightsail_container_service.tracking_api.url
}

output "principal_arn" {
  description = "Principal ARN of the container service"
  value       = aws_lightsail_container_service.tracking_api.principal_arn
}

output "private_domain_name" {
  description = "Private domain name of the container service"
  value       = aws_lightsail_container_service.tracking_api.private_domain_name
}

output "availability_zone" {
  description = "Availability zone of the container service"
  value       = aws_lightsail_container_service.tracking_api.availability_zone
}

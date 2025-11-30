output "certificate_arn" {
  description = "ARN of the SSL certificate"
  value       = aws_lightsail_certificate.tracking_api.arn
}

output "certificate_name" {
  description = "Name of the SSL certificate"
  value       = aws_lightsail_certificate.tracking_api.name
}

output "domain_validation_options" {
  description = "Domain validation options for the certificate"
  value       = aws_lightsail_certificate.tracking_api.domain_validation_options
}

output "record_name" {
  description = "DNS record name"
  value       = aws_route53_record.subdomain.name
}

output "record_fqdn" {
  description = "Fully qualified domain name"
  value       = aws_route53_record.subdomain.fqdn
}

output "lightsail_hostname" {
  description = "Lightsail container service hostname"
  value       = local.lightsail_hostname
}

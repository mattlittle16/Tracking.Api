# Extract hostname from Lightsail service URL for CNAME record
locals {
  # Remove https://, http://, and trailing slash from the service URL to get just the hostname
  lightsail_hostname = trimsuffix(replace(replace(var.lightsail_default_domain, "https://", ""), "http://", ""), "/")
}

# CNAME record pointing subdomain to Lightsail container service default domain
# This allows the custom domain to work with Lightsail's managed infrastructure
resource "aws_route53_record" "subdomain" {
  zone_id = var.hosted_zone_id
  name    = var.subdomain
  type    = "CNAME"
  ttl     = var.ttl

  # Points to Lightsail container service default domain
  # Format: <service-name>.<random-id>.us-east-1.cs.amazonlightsail.com
  records = [local.lightsail_hostname]
}

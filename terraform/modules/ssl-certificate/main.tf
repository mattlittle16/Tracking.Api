resource "aws_lightsail_certificate" "tracking_api" {
  name                      = var.certificate_name
  domain_name              = var.domain_name
  subject_alternative_names = var.subject_alternative_names

  tags = merge(var.tags, {
    Name = var.certificate_name
  })
}

# DNS validation records must be created manually or in a separate step
# The for_each loop cannot work because domain_validation_options is unknown until after apply
# 
# Option 1: Manual DNS records
#   After running terraform apply, get the validation records:
#   terraform output certificate_domain_validation_options
#   Then add CNAME records to Route53 manually
#
# Option 2: Targeted apply (recommended)
#   terraform apply -target=module.ssl_certificate.aws_lightsail_certificate.tracking_api
#   terraform apply (this will create DNS records below)

# DNS validation records - will only work on second apply
resource "aws_route53_record" "certificate_validation" {
  for_each = try({
    for dvo in aws_lightsail_certificate.tracking_api.domain_validation_options : dvo.domain_name => {
      name   = dvo.resource_record_name
      record = dvo.resource_record_value
      type   = dvo.resource_record_type
    }
  }, {})

  allow_overwrite = true
  name            = each.value.name
  records         = [each.value.record]
  ttl             = 60
  type            = each.value.type
  zone_id         = var.hosted_zone_id
}

# Tracking.Api AWS Lightsail Deployment

This directory contains Terraform configuration for deploying the Tracking API to AWS Lightsail Container Service.

## Prerequisites

- AWS CLI configured with appropriate credentials
- Terraform >= 1.0
- Docker
- Route53 hosted zone for your domain
- jq (for deployment script)

## Infrastructure Overview

The Terraform configuration creates:
- **ECR Repository**: For storing Docker images
- **Lightsail Container Service**: Runs the containerized API
- **SSL Certificate**: Managed SSL certificate for HTTPS
- **Route53 DNS**: CNAME record pointing to Lightsail service

## Initial Setup

### Before Terraform: Push Docker Image to ECR

You MUST create the ECR repository and push an initial image BEFORE running Terraform, otherwise Lightsail will fail when trying to pull a non-existent image.

**Option 1: Use the setup script (recommended)**
```bash
cd /Users/mattlittle/SourceControl/Tracking.Api
./setup-ecr.sh
```

**Option 2: Manual ECR setup**
```bash
# Create ECR repo
aws ecr create-repository --repository-name tracking-api --region us-east-1

# Login to ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 024955284766.dkr.ecr.us-east-1.amazonaws.com

# Build and push
cd Source
docker build --platform linux/amd64 -t tracking-api:latest -f Dockerfile .
docker tag tracking-api:latest 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest
docker push 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest
```

### Terraform Deployment

1. **Copy and configure terraform.tfvars**:
   ```bash
   cd terraform
   cp terraform.tfvars.example terraform.tfvars
   ```

2. **Edit terraform.tfvars** with your values:
   - `api_key`: Your API authentication key (required)
   - `domain_name`: Your custom domain (e.g., tracking-api.mattlittle.me)
   - `hosted_zone_id`: Your Route53 hosted zone ID
   - Keep `attach_custom_domain = false` initially

3. **Initialize Terraform**:
   ```bash
   terraform init
   ```

4. **Apply in TWO stages** (required due to SSL certificate limitation):
   
   **Stage 1 - Create SSL certificate:**
   ```bash
   terraform apply -target=module.ssl_certificate.aws_lightsail_certificate.tracking_api
   ```
   
   **Stage 2 - Create everything else:**
   ```bash
   terraform apply
   ```

   **Why two stages?** Terraform can't determine SSL validation DNS records until the certificate exists. The `-target` flag creates the certificate first, then the second apply can create the validation records.

## SSL Certificate Validation

After both Terraform applies complete:

1. The DNS validation records are automatically created in Route53 by Terraform

2. Wait for certificate validation (can take up to 30 minutes):
   ```bash
   # Check certificate status
   aws lightsail get-certificates --certificate-name tracking-api-prod-cert --region us-east-1
   
   # Or check validation records
   terraform output certificate_domain_validation_options
   ```

3. Once validated, enable custom domain:
   - Edit `terraform.tfvars` and set `attach_custom_domain = true`
   - Run `terraform apply` again

## Deploying Updates

After initial setup, use the provided deployment script to build, push, and deploy:

```bash
cd /Users/mattlittle/SourceControl/Tracking.Api
./dockerdeploy.sh
```

The script will:
1. Build Docker image for linux/amd64
2. Push to ECR
3. Run `terraform apply` to update infrastructure
4. Create new Lightsail deployment
5. Monitor deployment status
6. Display the service URL when complete

## Quick Reference: Complete First-Time Deployment

```bash
# 1. Setup ECR and push initial image
cd /Users/mattlittle/SourceControl/Tracking.Api
./setup-ecr.sh

# 2. Configure Terraform
cd terraform
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars and set your api_key

# 3. Initialize Terraform
terraform init

# 4. Apply in two stages
terraform apply -target=module.ssl_certificate.aws_lightsail_certificate.tracking_api
terraform apply

# 5. Wait ~30 mins for SSL validation, then enable custom domain
# Edit terraform.tfvars: attach_custom_domain = true
terraform apply

# 6. Your API is live!
# Check: terraform output api_url
```

## Quick Reference: Update Deployments

```bash
cd /Users/mattlittle/SourceControl/Tracking.Api
./dockerdeploy.sh
```

## Configuration Variables

Key variables in `variables.tf`:

- `service_name`: Lightsail service name (default: tracking-api)
- `container_power`: Container size (nano/micro/small/medium/large)
- `container_scale`: Number of instances (default: 1)
- `api_key`: API authentication key (sensitive)
- `ups_main_url`: UPS tracking main URL
- `ups_track_url`: UPS tracking API URL
- `channel_capacity`: Max tracking requests in queue (default: 1000)
- `cache_expiration_minutes`: Cache TTL (default: 5)
- `max_concurrent_processing`: Max concurrent requests (default: 10)

## Outputs

After deployment, Terraform provides:

```bash
terraform output api_url              # Your API URL
terraform output swagger_url          # Swagger UI URL
terraform output ecr_repository_url   # ECR repository URL
```

## Monitoring

Check deployment status:
```bash
aws lightsail get-container-service-deployments \
    --service-name tracking-api-prod \
    --region us-east-1
```

View container logs:
```bash
aws lightsail get-container-log \
    --service-name tracking-api-prod \
    --container-name tracking-api
```

## Cost Optimization

- Micro instance: ~$7-10/month
- ECR storage: Minimal (lifecycle policy keeps only 10 recent images)
- Data transfer: Included in Lightsail plan

## Troubleshooting

### Container fails health check
- Check logs: `aws lightsail get-container-log --service-name tracking-api-prod --container-name tracking-api`
- Verify health check path matches your API endpoint
- Ensure ASPNETCORE_URLS is set correctly

### Certificate validation stuck
- Verify Route53 has the validation records
- Check hosted zone ID is correct
- DNS propagation can take time

### Deployment timeout
- Check ECR image exists and is for linux/amd64
- Verify container service has ECR access role
- Review container logs for startup errors

## Cleanup

To destroy all resources:

```bash
cd terraform
terraform destroy
```

**Note**: This will delete the container service, ECR repository (and all images), and SSL certificate.

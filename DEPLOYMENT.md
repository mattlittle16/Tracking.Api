# Tracking.Api AWS Deployment Guide

## Deployment Strategy

This uses a **two-phase deployment** approach:
1. **Phase 1**: Create ECR repository, build and push initial image
2. **Phase 2**: Deploy Lightsail container service with the pushed image

---

## Phase 1: ECR Setup and Initial Image Push

### Step 1: Create ECR Repository Only

First, we need to create just the ECR repository without the Lightsail service.

**Option A: Using Terraform with targeted apply**
```bash
cd terraform

# Copy and configure terraform.tfvars
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars with your values

# Initialize Terraform
terraform init

# Create only the ECR repository
terraform apply -target=module.ecr
```

**Option B: Using AWS CLI (faster for initial setup)**
```bash
# Create ECR repository directly
aws ecr create-repository \
    --repository-name tracking-api \
    --region us-east-1 \
    --image-scanning-configuration scanOnPush=true

# Set lifecycle policy to clean up old images
aws ecr put-lifecycle-policy \
    --repository-name tracking-api \
    --region us-east-1 \
    --lifecycle-policy-text file://ecr-lifecycle-policy.json
```

### Step 2: Build and Push Initial Image

```bash
# Login to ECR
aws ecr get-login-password --region us-east-1 | \
    docker login --username AWS --password-stdin \
    024955284766.dkr.ecr.us-east-1.amazonaws.com

# Build the Docker image
cd /Users/mattlittle/SourceControl/Tracking.Api/Source
docker build --platform linux/amd64 -t tracking-api:latest -f Dockerfile .

# Tag and push to ECR
docker tag tracking-api:latest 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest
docker push 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest
```

### Step 3: Verify Image in ECR

```bash
# List images in ECR
aws ecr describe-images \
    --repository-name tracking-api \
    --region us-east-1
```

---

## Phase 2: Deploy Lightsail Container Service

Now that the image exists in ECR, deploy the full infrastructure:

### Step 1: Configure Terraform Variables

Edit `terraform/terraform.tfvars`:
```hcl
api_key = "your-secret-api-key"
attach_custom_domain = false  # Keep false initially
# ... other values
```

### Step 2: Apply Infrastructure Using Targeted Apply

Due to Terraform limitations with SSL certificates, use the `-target` approach:

**Step 2a: Create SSL certificate first**
```bash
cd terraform

terraform apply -target=module.ssl_certificate.aws_lightsail_certificate.tracking_api
```

**Step 2b: Apply everything else**
```bash
terraform apply
```

This creates:
- ✅ ECR repository (already exists, no changes)  
- 🆕 SSL certificate (step 2a)
- 🆕 DNS validation records (step 2b)
- 🆕 Lightsail container service (step 2b)
- 🆕 Route53 DNS records (step 2b)

### Step 3: Wait for SSL Certificate Validation

```bash
# Check certificate status
aws lightsail get-certificates \
    --certificate-name tracking-api-prod-cert \
    --region us-east-1

# Or check terraform output
terraform output certificate_domain_validation_options
```

DNS validation records are automatically created. Wait ~30 minutes for validation.

### Step 4: Enable Custom Domain

Once certificate is validated:

1. Edit `terraform.tfvars`:
   ```hcl
   attach_custom_domain = true
   ```

2. Apply changes:
   ```bash
   terraform apply
   ```

---

## Subsequent Deployments

After initial setup, use the deployment script:

```bash
# From Tracking.Api root directory
./dockerdeploy.sh
```

The script handles:
- Building the image
- Pushing to ECR
- Updating Lightsail deployment
- Monitoring deployment status

---

## Quick Start Commands

### First-Time Deployment

```bash
# 1. Create ECR and push image
cd /Users/mattlittle/SourceControl/Tracking.Api
aws ecr create-repository --repository-name tracking-api --region us-east-1 --image-scanning-configuration scanOnPush=true

aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 024955284766.dkr.ecr.us-east-1.amazonaws.com

cd Source
docker build --platform linux/amd64 -t tracking-api:latest -f Dockerfile .
docker tag tracking-api:latest 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest
docker push 024955284766.dkr.ecr.us-east-1.amazonaws.com/tracking-api:latest

# 2. Deploy infrastructure
cd ../terraform
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars with your api_key
terraform init
terraform apply -target=module.ssl_certificate.aws_lightsail_certificate.tracking_api
terraform apply

# 3. Wait for SSL validation (~30 mins)
# Then set attach_custom_domain = true in terraform.tfvars
terraform apply
```

### Updates After Initial Setup

```bash
./dockerdeploy.sh
```

---

## Troubleshooting

### "Image not found" error
- Ensure you pushed the image to ECR before applying Lightsail module
- Verify image exists: `aws ecr describe-images --repository-name tracking-api --region us-east-1`

### ECR already exists
- If you created ECR manually first, Terraform will import it
- Or use: `terraform import module.ecr.aws_ecr_repository.tracking_api tracking-api`

### Container fails to start
- Check logs: `aws lightsail get-container-log --service-name tracking-api-prod --container-name tracking-api`
- Verify environment variables in Lightsail module

---

## Alternative: Use Placeholder Image

If you want to use Terraform for everything from the start, you can:

1. Temporarily use a public placeholder image
2. Deploy infrastructure
3. Then push your real image and update

Edit `terraform/main.tf` line 49:
```hcl
# Temporarily use public image for initial deployment
container_image = "public.ecr.aws/docker/library/nginx:alpine"
# Then change to: "${module.ecr.repository_url}:${var.image_tag}"
```

This is NOT recommended as it adds extra steps.

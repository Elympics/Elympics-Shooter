# Cloudflare

terraform {
  required_providers {
    cloudflare = {
      source = "cloudflare/cloudflare"
      version = "~> 3.0"
    }
  }
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

# Cloudflare

resource "cloudflare_record" "cloudflare-a-record-stg" {
  zone_id = var.cloudflare_zone_id
  name    = "shooter-demo.elympics.cc"
  value   = var.staging_ip
  type    = "A"
  proxied = true
}

using System.Text.Json.Serialization;

namespace Tracking.Api.Core.Models;

public record UpsTrackResponse(
    [property: JsonPropertyName("statusCode")] string StatusCode,
    [property: JsonPropertyName("statusText")] string StatusText,
    [property: JsonPropertyName("isLoggedInUser")] bool IsLoggedInUser,
    [property: JsonPropertyName("trackedDateTime")] string TrackedDateTime,
    [property: JsonPropertyName("isBcdnMultiView")] bool IsBcdnMultiView,
    [property: JsonPropertyName("returnToDetails")] UpsTrackResponse.ReturnToDetailsData ReturnToDetails,
    [property: JsonPropertyName("trackDetails")] List<UpsTrackResponse.TrackDetailData> TrackDetails
)
{
    public record ReturnToDetailsData(
        [property: JsonPropertyName("returnToURL")] string? ReturnToURL,
        [property: JsonPropertyName("returnToApp")] string? ReturnToApp
    );

    public record TrackDetailData(
        [property: JsonPropertyName("errorCode")] string? ErrorCode,
        [property: JsonPropertyName("errorText")] string? ErrorText,
        [property: JsonPropertyName("requestedTrackingNumber")] string RequestedTrackingNumber,
        [property: JsonPropertyName("trackingNumber")] string TrackingNumber,
        [property: JsonPropertyName("isMobileDevice")] bool IsMobileDevice,
        [property: JsonPropertyName("packageStatus")] string PackageStatus,
        [property: JsonPropertyName("packageStatusType")] string PackageStatusType,
        [property: JsonPropertyName("packageStatusCode")] string PackageStatusCode,
        [property: JsonPropertyName("progressBarType")] string ProgressBarType,
        [property: JsonPropertyName("progressBarPercentage")] string ProgressBarPercentage,
        [property: JsonPropertyName("simplifiedText")] string SimplifiedText,
        [property: JsonPropertyName("endOfDayResCMSKey")] string? EndOfDayResCMSKey,
        [property: JsonPropertyName("receivedBy")] string ReceivedBy,
        [property: JsonPropertyName("leaveAt")] string? LeaveAt,
        [property: JsonPropertyName("nextExpectedEvent")] string? NextExpectedEvent,
        [property: JsonPropertyName("milestones")] List<Milestone> Milestones,
        [property: JsonPropertyName("isManifest")] bool IsManifest,
        [property: JsonPropertyName("isCommercialAddress")] bool IsCommercialAddress,
        [property: JsonPropertyName("altTrkNumInfo")] string? AltTrkNumInfo,
        [property: JsonPropertyName("alertCount")] int AlertCount,
        [property: JsonPropertyName("isEligibleViewMoreAlerts")] bool IsEligibleViewMoreAlerts,
        [property: JsonPropertyName("cdiLeaveAt")] string? CdiLeaveAt,
        [property: JsonPropertyName("leftAtActionCMSKey")] string LeftAtActionCMSKey,
        [property: JsonPropertyName("leftAt")] string LeftAt,
        [property: JsonPropertyName("showNoInfoDate")] bool ShowNoInfoDate,
        [property: JsonPropertyName("showPickupByDate")] bool ShowPickupByDate,
        [property: JsonPropertyName("shipToAddress")] Address ShipToAddress,
        [property: JsonPropertyName("shipFromAddress")] Address? ShipFromAddress,
        [property: JsonPropertyName("consigneeAddress")] Address? ConsigneeAddress,
        [property: JsonPropertyName("deliveryAddress")] DeliveryAddress DeliveryAddress,
        [property: JsonPropertyName("signatureImage")] string SignatureImage,
        [property: JsonPropertyName("trackHistoryDescription")] string? TrackHistoryDescription,
        [property: JsonPropertyName("additionalInformation")] AdditionalInformation AdditionalInformation,
        [property: JsonPropertyName("specialInstructions")] string? SpecialInstructions,
        [property: JsonPropertyName("proofOfDeliveryUrl")] string ProofOfDeliveryUrl,
        [property: JsonPropertyName("upsAccessPoint")] string? UpsAccessPoint,
        [property: JsonPropertyName("additionalPackagesCount")] int? AdditionalPackagesCount,
        [property: JsonPropertyName("attentionNeeded")] AttentionNeeded AttentionNeeded,
        [property: JsonPropertyName("shipmentProgressActivities")] List<ShipmentProgressActivity> ShipmentProgressActivities,
        [property: JsonPropertyName("shipmentGMTInfo")] ShipmentGMTInfo ShipmentGMTInfo,
        [property: JsonPropertyName("deliveryPhoto")] DeliveryPhoto DeliveryPhoto,
        [property: JsonPropertyName("trackingNumberType")] string TrackingNumberType,
        [property: JsonPropertyName("preAuthorizedForReturnData")] string? PreAuthorizedForReturnData,
        [property: JsonPropertyName("shipToAddressLblKey")] string ShipToAddressLblKey,
        [property: JsonPropertyName("isShipToAddressChangePending")] bool IsShipToAddressChangePending,
        [property: JsonPropertyName("trackSummaryView")] string? TrackSummaryView,
        [property: JsonPropertyName("senderShipperNumber")] string SenderShipperNumber,
        [property: JsonPropertyName("internalKey")] string InternalKey,
        [property: JsonPropertyName("deliveryOptions")] string? DeliveryOptions,
        [property: JsonPropertyName("upsellOptions")] string? UpsellOptions,
        [property: JsonPropertyName("sendUpdatesOptions")] string? SendUpdatesOptions,
        [property: JsonPropertyName("myChoiceUpSellLink")] string? MyChoiceUpSellLink,
        [property: JsonPropertyName("bcdnNumber")] string? BcdnNumber,
        [property: JsonPropertyName("promo")] Promo Promo,
        [property: JsonPropertyName("whatsNextText")] string? WhatsNextText,
        [property: JsonPropertyName("myChoiceToken")] string? MyChoiceToken,
        [property: JsonPropertyName("showMycTerms")] bool ShowMycTerms,
        [property: JsonPropertyName("enrollNum")] string EnrollNum,
        [property: JsonPropertyName("showConfirmWindow")] bool ShowConfirmWindow,
        [property: JsonPropertyName("confirmWindowLbl")] string? ConfirmWindowLbl,
        [property: JsonPropertyName("confirmWindowLink")] string? ConfirmWindowLink,
        [property: JsonPropertyName("fmdMap")] string? FmdMap,
        [property: JsonPropertyName("fileClaim")] string FileClaim,
        [property: JsonPropertyName("viewClaim")] string? ViewClaim,
        [property: JsonPropertyName("flightInformation")] string? FlightInformation,
        [property: JsonPropertyName("voyageInformation")] string? VoyageInformation,
        [property: JsonPropertyName("viewDeliveryReceipt")] string? ViewDeliveryReceipt,
        [property: JsonPropertyName("isInWatchList")] bool IsInWatchList,
        [property: JsonPropertyName("isHistoryUpdateRequire")] bool IsHistoryUpdateRequire,
        [property: JsonPropertyName("consumerHub")] string ConsumerHub,
        [property: JsonPropertyName("campusShip")] string? CampusShip,
        [property: JsonPropertyName("asrInformation")] AsrInformation AsrInformation,
        [property: JsonPropertyName("isSuppressDetailTab")] bool IsSuppressDetailTab,
        [property: JsonPropertyName("isUpsPremierPackage")] bool IsUpsPremierPackage,
        [property: JsonPropertyName("isUPSDeliveryPartner")] bool IsUPSDeliveryPartner,
        [property: JsonPropertyName("lastSensorLocation")] string? LastSensorLocation,
        [property: JsonPropertyName("lastSensorEnvInfo")] string? LastSensorEnvInfo,
        [property: JsonPropertyName("isPremierStyleEligible")] bool IsPremierStyleEligible,
        [property: JsonPropertyName("deliveryAttemptMsgDate")] string DeliveryAttemptMsgDate,
        [property: JsonPropertyName("isInFlight")] bool IsInFlight,
        [property: JsonPropertyName("shipmentUpsellEligible")] bool ShipmentUpsellEligible,
        [property: JsonPropertyName("ippaMessageInfo")] string? IppaMessageInfo,
        [property: JsonPropertyName("uploadDocumentsURL")] string UploadDocumentsURL,
        [property: JsonPropertyName("roadieURL")] string? RoadieURL,
        [property: JsonPropertyName("isDelivered")] bool IsDelivered,
        [property: JsonPropertyName("isDeliveredToUAP")] bool IsDeliveredToUAP,
        [property: JsonPropertyName("deliveredDayCMSKey")] string DeliveredDayCMSKey,
        [property: JsonPropertyName("deliveredDateDetail")] DateDetail DeliveredDateDetail,
        [property: JsonPropertyName("scheduledDeliveryDayCMSKey")] string ScheduledDeliveryDayCMSKey,
        [property: JsonPropertyName("scheduledDeliveryDateDetail")] DateDetail? ScheduledDeliveryDateDetail,
        [property: JsonPropertyName("packageStatusTime")] string PackageStatusTime,
        [property: JsonPropertyName("packageStatusTimeLbl")] string PackageStatusTimeLbl,
        [property: JsonPropertyName("isEDW")] bool IsEDW,
        [property: JsonPropertyName("disableSDDSection")] bool DisableSDDSection,
        [property: JsonPropertyName("unauthDrugCmsKey")] string? UnauthDrugCmsKey,
        [property: JsonPropertyName("isFraudIntercept")] bool IsFraudIntercept,
        [property: JsonPropertyName("hasBrokerageEvent")] bool HasBrokerageEvent,
        [property: JsonPropertyName("isMyChoiceCountry")] bool IsMyChoiceCountry,
        [property: JsonPropertyName("isMyChoicePkg")] bool IsMyChoicePkg,
        [property: JsonPropertyName("showText4ManifestSDD")] bool ShowText4ManifestSDD,
        [property: JsonPropertyName("stSDDSuppressionReason")] string StSDDSuppressionReason,
        [property: JsonPropertyName("showSignatureDeliveryOptionsMsg")] bool ShowSignatureDeliveryOptionsMsg,
        [property: JsonPropertyName("showMCDeliveryOptionsMsg")] bool ShowMCDeliveryOptionsMsg,
        [property: JsonPropertyName("showChangeDeliveryInfoMsg")] bool ShowChangeDeliveryInfoMsg,
        [property: JsonPropertyName("modifyDeliveryOptionsMsg")] string ModifyDeliveryOptionsMsg,
        [property: JsonPropertyName("isBasicOrSurepost")] bool IsBasicOrSurepost,
        [property: JsonPropertyName("isUpgradedSurepost")] bool IsUpgradedSurepost,
        [property: JsonPropertyName("isSmartPackage")] bool IsSmartPackage,
        [property: JsonPropertyName("hasMC4HToken")] bool HasMC4HToken,
        [property: JsonPropertyName("ucixURL")] string UcixURL,
        [property: JsonPropertyName("proximityMap")] ProximityMap ProximityMap,
        [property: JsonPropertyName("isSecurityCodeRequested")] bool IsSecurityCodeRequested,
        [property: JsonPropertyName("sddMsg")] string SddMsg,
        [property: JsonPropertyName("receiverStopType")] string ReceiverStopType,
        [property: JsonPropertyName("sdd")] string? Sdd,
        [property: JsonPropertyName("sdt")] string? Sdt,
        [property: JsonPropertyName("isDelayedPackge")] bool IsDelayedPackge,
        [property: JsonPropertyName("milestoneCount")] int MilestoneCount,
        [property: JsonPropertyName("currentMilestone")] string? CurrentMilestone,
        [property: JsonPropertyName("milestoneList")] string? MilestoneList
    );

    public record Milestone(
        [property: JsonPropertyName("isCurrent")] bool IsCurrent,
        [property: JsonPropertyName("isCompleted")] bool IsCompleted,
        [property: JsonPropertyName("isFuture")] bool IsFuture,
        [property: JsonPropertyName("isRFIDIcon")] bool IsRFIDIcon,
        [property: JsonPropertyName("category")] string? Category,
        [property: JsonPropertyName("subMilestone")] string? SubMilestone,
        [property: JsonPropertyName("returnTrackingNumber")] string? ReturnTrackingNumber,
        [property: JsonPropertyName("pTrackingNumber")] string? PTrackingNumber,
        [property: JsonPropertyName("cTrackingNumber")] string? CTrackingNumber,
        [property: JsonPropertyName("date")] string Date,
        [property: JsonPropertyName("time")] string Time,
        [property: JsonPropertyName("location")] string Location,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("nameKey")] string NameKey
    );

    public record Address(
        [property: JsonPropertyName("streetAddress1")] string StreetAddress1,
        [property: JsonPropertyName("streetAddress2")] string StreetAddress2,
        [property: JsonPropertyName("streetAddress3")] string StreetAddress3,
        [property: JsonPropertyName("city")] string City,
        [property: JsonPropertyName("state")] string State,
        [property: JsonPropertyName("province")] string? Province,
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("zipCode")] string ZipCode,
        [property: JsonPropertyName("companyName")] string CompanyName,
        [property: JsonPropertyName("attentionName")] string AttentionName,
        [property: JsonPropertyName("isAddressCorrected")] bool IsAddressCorrected,
        [property: JsonPropertyName("isReturnAddress")] bool IsReturnAddress,
        [property: JsonPropertyName("isHoldAddress")] bool IsHoldAddress
    );

    public record DeliveryAddress(
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("zipCode")] string? ZipCode
    );

    public record AdditionalInformation(
        [property: JsonPropertyName("serviceInformation")] ServiceInformation ServiceInformation,
        [property: JsonPropertyName("weight")] string Weight,
        [property: JsonPropertyName("weightUnit")] string? WeightUnit,
        [property: JsonPropertyName("codInformation")] string? CodInformation,
        [property: JsonPropertyName("shippedOrBilledDate")] string ShippedOrBilledDate,
        [property: JsonPropertyName("referenceNumbers")] string? ReferenceNumbers,
        [property: JsonPropertyName("postalServiceTrackingID")] string PostalServiceTrackingID,
        [property: JsonPropertyName("alternateTrackingNumbers")] string? AlternateTrackingNumbers,
        [property: JsonPropertyName("otherRequestedServices")] string? OtherRequestedServices,
        [property: JsonPropertyName("descriptionOfGood")] string DescriptionOfGood,
        [property: JsonPropertyName("cargoReady")] string CargoReady,
        [property: JsonPropertyName("fileNumber")] string FileNumber,
        [property: JsonPropertyName("originPort")] string OriginPort,
        [property: JsonPropertyName("destinationPort")] string DestinationPort,
        [property: JsonPropertyName("estimatedArrival")] string EstimatedArrival,
        [property: JsonPropertyName("estimatedDeparture")] string EstimatedDeparture,
        [property: JsonPropertyName("poNumber")] string PoNumber,
        [property: JsonPropertyName("blNumber")] string BlNumber,
        [property: JsonPropertyName("appointmentMade")] string? AppointmentMade,
        [property: JsonPropertyName("appointmentRequested")] string? AppointmentRequested,
        [property: JsonPropertyName("appointmentRequestedRange")] string? AppointmentRequestedRange,
        [property: JsonPropertyName("manifest")] string Manifest,
        [property: JsonPropertyName("isSmallPackage")] bool IsSmallPackage,
        [property: JsonPropertyName("shipmentVolume")] string? ShipmentVolume,
        [property: JsonPropertyName("numberOfPallets")] string? NumberOfPallets,
        [property: JsonPropertyName("shipmentCategory")] string ShipmentCategory,
        [property: JsonPropertyName("pkgSequenceNum")] string? PkgSequenceNum,
        [property: JsonPropertyName("pkgIdentificationCode")] string? PkgIdentificationCode,
        [property: JsonPropertyName("pkgID")] string? PkgID,
        [property: JsonPropertyName("product")] string? Product,
        [property: JsonPropertyName("numberOfPieces")] string? NumberOfPieces,
        [property: JsonPropertyName("wwef")] bool Wwef,
        [property: JsonPropertyName("wwePostal")] bool WwePostal,
        [property: JsonPropertyName("showAltTrkLink")] bool ShowAltTrkLink,
        [property: JsonPropertyName("wweParcel")] bool WweParcel,
        [property: JsonPropertyName("deliveryPreference")] string? DeliveryPreference,
        [property: JsonPropertyName("liftGateOnDelivery")] string? LiftGateOnDelivery,
        [property: JsonPropertyName("liftGateOnPickup")] string? LiftGateOnPickup,
        [property: JsonPropertyName("pickupDropOffDate")] string? PickupDropOffDate,
        [property: JsonPropertyName("pickupPreference")] string? PickupPreference
    );

    public record ServiceInformation(
        [property: JsonPropertyName("serviceName")] string ServiceName,
        [property: JsonPropertyName("serviceLink")] string? ServiceLink,
        [property: JsonPropertyName("serviceAttribute")] string? ServiceAttribute
    );

    public record AttentionNeeded(
        [property: JsonPropertyName("actions")] List<string> Actions,
        [property: JsonPropertyName("isCorrectMyAddress")] bool IsCorrectMyAddress
    );

    public record ShipmentProgressActivity(
        [property: JsonPropertyName("date")] string Date,
        [property: JsonPropertyName("time")] string Time,
        [property: JsonPropertyName("location")] string Location,
        [property: JsonPropertyName("activityScan")] string ActivityScan,
        [property: JsonPropertyName("milestoneName")] MilestoneName? MilestoneName,
        [property: JsonPropertyName("isInOverViewTable")] bool IsInOverViewTable,
        [property: JsonPropertyName("activityAdditionalDescription")] string? ActivityAdditionalDescription,
        [property: JsonPropertyName("trailer")] string Trailer,
        [property: JsonPropertyName("isDisplayPodLink")] bool IsDisplayPodLink,
        [property: JsonPropertyName("isRFIDIconEvent")] bool IsRFIDIconEvent,
        [property: JsonPropertyName("actCode")] string ActCode,
        [property: JsonPropertyName("exceptionCodes")] string? ExceptionCodes,
        [property: JsonPropertyName("gmtDate")] string GmtDate,
        [property: JsonPropertyName("gmtOffset")] string GmtOffset,
        [property: JsonPropertyName("gmtTime")] string GmtTime,
        [property: JsonPropertyName("alternateTrackingNumbers")] string? AlternateTrackingNumbers,
        [property: JsonPropertyName("isBrokerageEvent")] bool IsBrokerageEvent
    );

    public record MilestoneName(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("nameKey")] string NameKey
    );

    public record ShipmentGMTInfo(
        [property: JsonPropertyName("shipFromGMTOffset")] string ShipFromGMTOffset,
        [property: JsonPropertyName("shipToGMTOffset")] string ShipToGMTOffset
    );

    public record DeliveryPhoto(
        [property: JsonPropertyName("isAuthFlow")] bool IsAuthFlow,
        [property: JsonPropertyName("isNonPostalCodeCountry")] bool IsNonPostalCodeCountry,
        [property: JsonPropertyName("viewable")] bool Viewable,
        [property: JsonPropertyName("photoResponse")] string? PhotoResponse
    );

    public record Promo(
        [property: JsonPropertyName("isPackagePromotion")] bool IsPackagePromotion,
        [property: JsonPropertyName("isShipperPromotion")] bool IsShipperPromotion,
        [property: JsonPropertyName("productImage")] string? ProductImage,
        [property: JsonPropertyName("title")] string? Title,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("shipperURL")] string? ShipperURL,
        [property: JsonPropertyName("shipperLogoURL")] string? ShipperLogoURL
    );

    public record AsrInformation(
        [property: JsonPropertyName("allowDriverRelease")] string? AllowDriverRelease,
        [property: JsonPropertyName("processEDN")] string? ProcessEDN
    );

    public record DateDetail(
        [property: JsonPropertyName("monthCMSKey")] string MonthCMSKey,
        [property: JsonPropertyName("dayNum")] string DayNum
    );

    public record ProximityMap(
        [property: JsonPropertyName("mapStatus")] string MapStatus,
        [property: JsonPropertyName("guestStatus")] string GuestStatus,
        [property: JsonPropertyName("imageLocationLatitude")] string? ImageLocationLatitude,
        [property: JsonPropertyName("imageLocationLongitude")] string? ImageLocationLongitude,
        [property: JsonPropertyName("deliveryAddressLocationLatitude")] string? DeliveryAddressLocationLatitude,
        [property: JsonPropertyName("deliveryAddressLocationLongitude")] string? DeliveryAddressLocationLongitude,
        [property: JsonPropertyName("distance")] string? Distance,
        [property: JsonPropertyName("deliveredToAddress")] string? DeliveredToAddress
    );
}

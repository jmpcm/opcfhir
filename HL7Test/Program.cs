using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace HL7Test
{
    class Program
    {
        static void Main(string[] args)
        {
            const string kDeviceId = "device-0001";
            const string kObservationId = "bp-0001";
            const string kPatientId = "patient-0001";
            const string kPractitionerId = "practitioner-0001";

            Patient patient = new Patient()
            {
                Id = kPatientId
            };
            patient.Name.Add(HumanName.ForFamily("Miranda").WithGiven("Jorge"));

            Practitioner practitioner = new Practitioner()
            {
                Id = kPractitionerId
            };
            practitioner.Name.Add(HumanName.ForFamily("Banerjee").WithGiven("Suprateek"));

			Device device = new Device()
			{
                Id = kDeviceId,
				Manufacturer = "A&D",
                Model = "UA-651BLE"
            };

            Observation observation = new Observation()
            {
                Id = kObservationId,
                Device = new ResourceReference("Device/" + kDeviceId),
                Subject = new ResourceReference("Patient" + kPatientId)
            };
            observation.Performer.Add(new ResourceReference("Practitioner" + kPractitionerId));

            /* Systolic component */
            Observation.ComponentComponent systolic = new Observation.ComponentComponent();
            systolic.Code = new CodeableConcept();
            systolic.Code.Coding.Add(new Coding("http://loinc.org", "8480 - 6"));
            systolic.Code.Coding.Add(new Coding("http://snomed.info/sct", "271649006"));
            systolic.Value = new Quantity()
            {
                Value = 107,
                Unit = "mmHg",
                Code = "mm[Hg]"
            };
            systolic.Interpretation = new CodeableConcept("http://hl7.org/fhir/v2/0078", "N", "normal");

            /* Diastolic component */
			Observation.ComponentComponent diastolic = new Observation.ComponentComponent();
            diastolic.Code = new CodeableConcept("http://loinc.org","8462-4");
            diastolic.Code = new CodeableConcept("http://snomed.info/sct", "271650006");
            diastolic.Value = new Quantity()
			{
				Value = 60,
				Unit = "mmHg",
                Code = "mm[Hg]"
			};
            diastolic.Interpretation = new CodeableConcept("http://hl7.org/fhir/v2/0078", "L", "low");
            			
            observation.Component.Add(systolic);
            observation.Component.Add(diastolic);

            string json = FhirSerializer.SerializeToJson(observation);
        }
    }
}

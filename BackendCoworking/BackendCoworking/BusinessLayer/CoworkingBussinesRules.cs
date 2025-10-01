using BackendCoworking.Models;
using BackendCoworking.Models.DTOs;
using System.Text.RegularExpressions;

namespace BackendCoworking.BusinessLayer
{
    public static class CoworkingBussinesRules
    {
        public static string GetAvailabilitySummary(Coworking coworking)
        {
            int privateRooms = 0;
            int desks = 0;
            int meetingRooms = 0;
            string summary = "";

            var workspaces = coworking.Workspaces;

            foreach (Workspaces workspace in workspaces)
            {
                workspace.WorkspaceAvailabilitys.ToList().ForEach(wa =>
                {
                    string availabilityName = wa.Availability.Name.ToLower();
                    Match match = Regex.Match(availabilityName, @"(\d+)");

                    if (match.Success)
                    {
                        int count = int.Parse(match.Groups[1].Value);

                        if (availabilityName.Contains("meeting rooms") || availabilityName.Contains("meeting room"))
                        {
                            meetingRooms += count;
                        }
                        else if (availabilityName.Contains("private rooms") || availabilityName.Contains("private room"))
                        {
                            privateRooms += count;
                        }
                        else if (availabilityName.Contains("desks") || availabilityName.Contains("desk"))
                        {
                            desks += count;
                        }
                        else if (availabilityName.Contains("rooms") || availabilityName.Contains("room"))
                        {
                            if (!availabilityName.Contains("no"))
                            {
                                privateRooms += count;
                            }
                        }
                    }
                });
            }
            var summaryParts = new List<string>();

            if (desks > 0)
            {
                summaryParts.Add($"🪑 {desks} desks");
            }

            if (privateRooms > 0)
            {
                summaryParts.Add($"🔒 {privateRooms} private rooms");
            }

            if (meetingRooms > 0)
            {
                summaryParts.Add($"👥 {meetingRooms} meeting rooms");
            }

            summary = string.Join(" · ", summaryParts);
            return summary;
        }
    }
}

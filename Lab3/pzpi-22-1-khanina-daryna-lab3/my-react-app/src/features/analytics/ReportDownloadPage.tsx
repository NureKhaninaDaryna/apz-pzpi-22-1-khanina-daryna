import { useState } from 'react';
import { Button, Form, Header, Input, Segment, Grid } from 'semantic-ui-react';
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store.ts";

export default observer(function ReportDownloadPage() {
   const [startDate, setStartDate] = useState<Date | undefined>();
   const [endDate, setEndDate] = useState<Date | undefined>();
   const { analyticsStore } = useStore();

   const handleDownloadReport = async (start?: Date, end?: Date) => {
      try {
         const response = await analyticsStore.downloadReport(start, end);
         const file = new Blob([response!], { type: 'application/pdf' });
         const fileURL = URL.createObjectURL(file);
         const a = document.createElement('a');
         a.href = fileURL;
         a.download = `Reports_${start?.toISOString().slice(0, 10)}_to_${end?.toISOString().slice(0, 10)}.pdf`;
         a.click();
      } catch (error) {
         console.error("Error downloading report:", error);
      }
   };

   const getToday = () => new Date();

   const getLastWeekRange = () => {
      const end = new Date();
      const start = new Date();
      start.setDate(end.getDate() - 6);
      return { start, end };
   };

   const getLastMonthRange = () => {
      const end = new Date();
      const start = new Date(end.getFullYear(), end.getMonth(), 1);
      return { start, end };
   };

   return (
      <Segment>
         <Header as="h2" content="Download Reports" dividing />

         <Grid stackable columns={2}>
            <Grid.Column>
               <Header as="h4" content="Quick Reports" />
               <p>Download predefined reports:</p>
               <Button
                  fluid
                  color="teal"
                  onClick={() => {
                     const today = getToday();
                     handleDownloadReport(today, today);
                  }}
               >
                  Daily Report
               </Button>
               <Button
                  fluid
                  color="blue"
                  style={{ marginTop: '0.5em' }}
                  onClick={() => {
                     const { start, end } = getLastWeekRange();
                     handleDownloadReport(start, end);
                  }}
               >
                  Weekly Report
               </Button>
               <Button
                  fluid
                  color="purple"
                  style={{ marginTop: '0.5em' }}
                  onClick={() => {
                     const { start, end } = getLastMonthRange();
                     handleDownloadReport(start, end);
                  }}
               >
                  Monthly Report
               </Button>
            </Grid.Column>

            <Grid.Column>
               <Header as="h4" content="Custom Date Range" />
               <Form loading={analyticsStore.loading}>
                  <Form.Field>
                     <label>Start Date</label>
                     <Input
                        type="date"
                        value={startDate?.toISOString().slice(0, 10) || ''}
                        onChange={(e) => setStartDate(new Date(e.target.value))}
                     />
                  </Form.Field>
                  <Form.Field>
                     <label>End Date</label>
                     <Input
                        type="date"
                        value={endDate?.toISOString().slice(0, 10) || ''}
                        onChange={(e) => setEndDate(new Date(e.target.value))}
                     />
                  </Form.Field>
                  <Button
                     onClick={() => handleDownloadReport(startDate, endDate)}
                     primary
                     fluid
                     loading={analyticsStore.loading}
                     disabled={!startDate || !endDate}
                  >
                     Download Custom Report
                  </Button>
               </Form>
            </Grid.Column>
         </Grid>
      </Segment>
   );
});

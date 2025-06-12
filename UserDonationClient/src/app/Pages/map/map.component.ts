import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent implements AfterViewInit {
  @ViewChild('mapContainer', { static: false }) mapElement!: ElementRef;

  map: google.maps.Map | null = null;
  heatmap: google.maps.visualization.HeatmapLayer | null = null;

  heatmapData: google.maps.LatLngLiteral[] = [];
  center: google.maps.LatLngLiteral = { lat: 10.8380623, lng: 106.646811 };
  zoom = 10;
  heatmapOpacity = 0.6;

  // API URL
  private apiUrl = 'http://localhost:5178/Get-locations/';

  // Ngày bắt đầu và kết thúc
  startDate: string = '';
  endDate: string = '';

  isDateRangeValid: boolean = false; // Trạng thái hợp lệ của khoảng ngày

  constructor(private http: HttpClient) { }

  ngAfterViewInit() {
    this.initializeMap();
    this.loadLocationsByDateRange();
  }
  // Khởi tạo bản đồ
  initializeMap() {
    this.map = new google.maps.Map(this.mapElement.nativeElement, {
      center: this.center,
      zoom: this.zoom,
    });
  }

  // Kiểm tra tính hợp lệ của khoảng ngày
  validateDateRange() {
    if (this.startDate && this.endDate) {
      this.isDateRangeValid = new Date(this.startDate) <= new Date(this.endDate);
    } else {
      this.isDateRangeValid = false;
    }
  }
  // Gọi API để lấy dữ liệu theo khoảng ngày
  loadLocationsByDateRange() {
    if (!this.isDateRangeValid) return;

    // Convert startDate and endDate to ISO string (YYYY-MM-DDT00:00:00)
    const startDateISO = new Date(this.startDate).toISOString();
    const endDateISO = new Date(this.endDate).toISOString();

    const apiUrlWithDates = `${this.apiUrl}?startDate=${startDateISO}&endDate=${endDateISO}`;
    console.log('Requesting data from API:', apiUrlWithDates);

    this.http.get<any>(apiUrlWithDates).subscribe(
      (response) => {
        console.log('API response:', response);

        // Kiểm tra và lọc dữ liệu từ `$values`
        const locations = response?.$values || [];
        this.heatmapData = locations.map((loc: any) => ({
          lat: loc.latitude,
          lng: loc.longitude,
        }));

        console.log('Heatmap data after update:', this.heatmapData);

        // Reset và cập nhật heatmap
        this.resetHeatmap();
        this.updateHeatmap();
      },
      (error) => {
        console.error('Error loading locations:', error);
      }
    );
  }
  //Reset
  resetHeatmap() {
    if (this.heatmap) {
      this.heatmap.setMap(null); // Xóa heatmap cũ khỏi bản đồ
      this.heatmap = null; // Đặt về null để khởi tạo lại
    }
  }
  // Cập nhật heatmap
  updateHeatmap() {
    if (!this.map) return;

    // Nếu heatmap đã tồn tại, loại bỏ trước khi tạo mới
    if (this.heatmap) {
      this.heatmap.setMap(null); // Xóa heatmap cũ
    }

    // Tạo heatmap mới với dữ liệu cập nhật
    this.heatmap = new google.maps.visualization.HeatmapLayer({
      data: this.heatmapData.map((point) => new google.maps.LatLng(point.lat, point.lng)),
      opacity: this.heatmapOpacity,
    });

    // Gắn heatmap mới vào bản đồ
    this.heatmap.setMap(this.map);
  }

  // Điều chỉnh độ trong suốt của heatmap
  adjustHeatmapOpacity(event: Event) {
    const input = event.target as HTMLInputElement;
    this.heatmapOpacity = parseFloat(input.value);
    if (this.heatmap) {
      this.heatmap.setOptions({ opacity: this.heatmapOpacity });
    }
  }
}

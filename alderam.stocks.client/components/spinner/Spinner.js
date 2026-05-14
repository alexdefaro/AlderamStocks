'use client';

export default function Spinner({ visibilityValue }) {
  return (
    <div className="spinner" style={{ visibility: visibilityValue }}>
      <svg style={{ margin: 'auto', background: 'none', display: 'block' }}
        width="200px" height="200px" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid">
        <g>
          <circle cx="88.8096" cy="50" r="4" fill="#f0ad4e">
            <animate attributeName="cx" repeatCount="indefinite" dur="1s" values="95;35" keyTimes="0;1" begin="-0.67s" />
            <animate attributeName="fill-opacity" repeatCount="indefinite" dur="1s" values="0;1;1" keyTimes="0;0.2;1" begin="-0.67s" />
          </circle>
          <circle cx="49.2096" cy="50" r="4" fill="#f0ad4e">
            <animate attributeName="cx" repeatCount="indefinite" dur="1s" values="95;35" keyTimes="0;1" begin="-0.33s" />
            <animate attributeName="fill-opacity" repeatCount="indefinite" dur="1s" values="0;1;1" keyTimes="0;0.2;1" begin="-0.33s" />
          </circle>
          <circle cx="69.0096" cy="50" r="4" fill="#f0ad4e">
            <animate attributeName="cx" repeatCount="indefinite" dur="1s" values="95;35" keyTimes="0;1" begin="0s" />
            <animate attributeName="fill-opacity" repeatCount="indefinite" dur="1s" values="0;1;1" keyTimes="0;0.2;1" begin="0s" />
          </circle>
        </g>
        <g transform="translate(-15 0)">
          <path d="M50 50L20 50A30 30 0 0 0 80 50Z" fill="#5bc0de" transform="rotate(90 50 50)" />
          <path d="M50 50L20 50A30 30 0 0 0 80 50Z" fill="#5bc0de" transform="rotate(38.9857 50 50)">
            <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;45 50 50;0 50 50" keyTimes="0;0.5;1" />
          </path>
          <path d="M50 50L20 50A30 30 0 0 1 80 50Z" fill="#5bc0de" transform="rotate(-38.9857 50 50)">
            <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;-45 50 50;0 50 50" keyTimes="0;0.5;1" />
          </path>
        </g>
      </svg>
    </div>
  );
}

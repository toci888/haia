import type { PropsWithChildren } from 'react'
import styles from './studio-layout.module.css'
import { StudioHeader } from './StudioHeader'

export function StudioLayout({ children }: PropsWithChildren): React.ReactElement {
	return (
		<div className={styles.page}>
			<StudioHeader />
			<main className={styles.main}>{children}</main>
		</div>
	)
}
